using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Cryptography.Pkcs;

namespace ApiProjectCamp.WebUI.Controllers
{
    public class AIController : Controller
    {
        public IActionResult CreateRecipeWithOpenAI()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecipeWithOpenAI(string prompt)
        {
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
                        content = "Sen bir restoran için yemek önerileri yapan bir yapay zeka aracısın. Amacımız kullanıcı tarafından girilen malzemelere göre yemek tarifi önerisinde bılunmak."
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
                ViewBag.Recipe = content;
            }
            else
            {
                ViewBag.RecipeError = "Bir hata oluştu : " + response.StatusCode;
            }

            return View();
        }

        public class OpenAIResponse
        {
            public List<Choice> Choices { get; set; }
        }

        public class Choice
        {
            public Message Message { get; set; }
        }

        public class Message
        {
            public string Role { get; set; }
            public string Content { get; set; }
        }
    }
}
