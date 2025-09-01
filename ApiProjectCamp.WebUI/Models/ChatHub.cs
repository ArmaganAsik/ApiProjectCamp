using Microsoft.AspNetCore.SignalR;
using Microsoft.DiaSymReader;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiProjectCamp.WebUI.Models
{
    public class ChatHub : Hub
    {
        private const string apiKey = ""; //openai api key here
        private const string modelGpt = "gpt-4o-mini";
        private readonly IHttpClientFactory _httpClientFactory;

        public ChatHub(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private static readonly Dictionary<string, List<Dictionary<string, string>>> _history = new();

        public override Task OnConnectedAsync()
        {
            _history[Context.ConnectionId] = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    ["role"] = "system",
                    ["content"] = "You are a helpful assistant. Keep answers concise."
                }
            };

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _history.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string userMessage)
        {
            await Clients.Caller.SendAsync("ReceiveUserEcho", userMessage);
            List<Dictionary<string, string>> history = _history[Context.ConnectionId];
            history.Add(new()
            {
                ["role"] = "user",
                ["content"] = userMessage
            });
            await StreamOpenAI(history, Context.ConnectionAborted);
        }

        public async Task StreamOpenAI(List<Dictionary<string, string>> history, CancellationToken cancellationToken)
        {
            HttpClient client = _httpClientFactory.CreateClient("openai");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var payload = new
            {
                model = modelGpt,
                messages = history,
                stream = true,
                temperature = 0.2
            };

            using HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, "v1/chat/completions");
            req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            using HttpResponseMessage resp = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            resp.EnsureSuccessStatusCode();

            using Stream stream = await resp.Content.ReadAsStreamAsync(cancellationToken);
            using StreamReader reader = new StreamReader(stream);

            StringBuilder sb = new StringBuilder();
            while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
            {
                string line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (!line.StartsWith("data:")) continue;
                string data = line["data:".Length..].Trim();
                if (data == "[DONE]") break;

                try
                {
                    ChatStreamChunk chunk = JsonSerializer.Deserialize<ChatStreamChunk>(data);
                    string delta = chunk?.Choices?.FirstOrDefault()?.Delta?.Content;
                    if (!string.IsNullOrEmpty(delta))
                    {
                        sb.Append(delta);
                        await Clients.Caller.SendAsync("ReceiveToken", delta, cancellationToken);
                    }
                }
                catch
                {

                    //Error Messages
                }
            }

            string full = sb.ToString();
            history.Add(new()
            {
                ["role"] = "assistant",
                ["content"] = full
            });
            await Clients.Caller.SendAsync("CompleteMessage", full, cancellationToken);
        }

        private sealed class ChatStreamChunk
        {
            [JsonPropertyName("choices")] public List<Choice>? Choices { get; set; }
        }

        private sealed class Choice
        {
            [JsonPropertyName("delta")] public Delta? Delta { get; set; }
            [JsonPropertyName("finish_reason")] public string? FinishReason { get; set; }
        }

        private sealed class Delta
        {
            [JsonPropertyName("role")] public string? Role { get; set; }
            [JsonPropertyName("content")] public string? Content { get; set; }
        }
    }
}
