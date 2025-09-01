using ApiProjectCamp.WebUI.Dtos.MessageDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static ApiProjectCamp.WebUI.Controllers.AIController;

namespace ApiProjectCamp.WebUI.Controllers
{
    public class MessageController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MessageController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> ListMessages()
        {
            HttpClient client = _httpClientFactory.CreateClient();
            HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7258/api/Messages");
            if (responseMessage.IsSuccessStatusCode)
            {
                string jsonData = await responseMessage.Content.ReadAsStringAsync();
                List<ResultMessageDto> values = JsonConvert.DeserializeObject<List<ResultMessageDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        public IActionResult CreateMessage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(CreateMessageDto createMessageDto)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            string jsonData = JsonConvert.SerializeObject(createMessageDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync("https://localhost:7258/api/Messages", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("ListMessages");
            }
            return View();
        }

        public async Task<IActionResult> DeleteMessage(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            await client.DeleteAsync("https://localhost:7258/api/Messages?id=" + id);
            return RedirectToAction("ListMessages");
        }

        public async Task<IActionResult> UpdateMessage(int id)
        {
            HttpResponseMessage responseMessage = await _httpClientFactory.CreateClient().GetAsync("https://localhost:7258/api/Messages/GetMessageById?id=" + id);
            string jsonData = await responseMessage.Content.ReadAsStringAsync();
            GetByIdMessageDto value = JsonConvert.DeserializeObject<GetByIdMessageDto>(jsonData);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMessage(UpdateMessageDto updateMessageDto)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            string jsonData = JsonConvert.SerializeObject(updateMessageDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            await client.PutAsync("https://localhost:7258/api/Messages", stringContent);
            return RedirectToAction("ListMessages");
        }

        [HttpGet]
        public async Task<IActionResult> AnswerMessageWithOpenAI(int id, string prompt)
        {
            HttpResponseMessage responseMessage = await _httpClientFactory.CreateClient().GetAsync("https://localhost:7258/api/Messages/GetMessageById?id=" + id);
            string jsonData = await responseMessage.Content.ReadAsStringAsync();
            GetByIdMessageDto value = JsonConvert.DeserializeObject<GetByIdMessageDto>(jsonData);

            prompt = value.MessageDetails;

            string apiKey = ""; //openai api key here

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var requestData = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content = "Sen bir restoran için kullanıcıların göndermiş oldukları mesajlara detaylı ve olabildiğinde olumlu, müşteri memnuniyetini gözeten cevaplar veren bir yapay zeka aracısın. Amacımız kullanıcı tarafından gönderilen mesajlara en olumlu ve mantıklı cevapları sunabilmek."
                    },
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                },
                temperature = 0.5
            };

            HttpResponseMessage response = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestData);

            if (response.IsSuccessStatusCode)
            {
                OpenAIResponse result = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
                string content = result.Choices[0].Message.Content;
                ViewBag.AnswerAI = content;
            }
            else
            {
                ViewBag.AnswerAI = "Bir hata oluştu : " + response.StatusCode;
            }

            return View(value);
        }

        public PartialViewResult SendMessage()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(CreateMessageDto createMessageDto)
        {
            HttpClient client = new HttpClient();
            string apiKey = ""; //huggingface api key here
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            try
            {
                var translateRequestBody = new
                {
                    inputs = createMessageDto.MessageDetails
                };
                string translateJson = System.Text.Json.JsonSerializer.Serialize(translateRequestBody);
                StringContent translateContent = new StringContent(translateJson, Encoding.UTF8, "application/json");

                HttpResponseMessage translateResponse = await client.PostAsync("https://api-inference.huggingface.co/models/Helsinki-NLP/opus-mt-tr-en", translateContent);
                string translateResponseString = await translateResponse.Content.ReadAsStringAsync();

                string englishText = createMessageDto.MessageDetails;
                if (translateResponseString.TrimStart().StartsWith("["))
                {
                    JsonDocument translateDoc = JsonDocument.Parse(translateResponseString);
                    englishText = translateDoc.RootElement[0].GetProperty("translation_text").GetString();
                    //ViewBag.EnglishText = englishText;
                }

                var toxicRequestBody = new
                {
                    inputs = englishText
                };

                string toxicJson = System.Text.Json.JsonSerializer.Serialize(toxicRequestBody);
                StringContent toxicContent = new StringContent(toxicJson, Encoding.UTF8, "application/json");
                HttpResponseMessage toxicResponse = await client.PostAsync("https://api-inference.huggingface.co/models/unitary/toxic-bert", toxicContent);
                string toxicResponseString = await toxicResponse.Content.ReadAsStringAsync();

                if (toxicResponseString.TrimStart().StartsWith("["))
                {
                    JsonDocument toxicDoc = JsonDocument.Parse(toxicResponseString);
                    foreach (JsonElement item in toxicDoc.RootElement[0].EnumerateArray())
                    {
                        string label = item.GetProperty("label").GetString();
                        double score = item.GetProperty("score").GetDouble();

                        if (score > 0.5)
                        {
                            createMessageDto.Status = "Toksik Mesaj";
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(createMessageDto.Status))
                {
                    createMessageDto.Status = "Mesaj Alındı";
                }
            }
            catch
            {
                createMessageDto.Status = "Onay Bekliyor";
            }

            HttpClient clientM = _httpClientFactory.CreateClient();
            string jsonData = JsonConvert.SerializeObject(createMessageDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await clientM.PostAsync("https://localhost:7258/api/Messages", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("ListMessages");
            }
            return View();
        }
    }
}
