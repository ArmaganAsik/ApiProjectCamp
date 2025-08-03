using ApiProjectCamp.WebUI.Dtos.MessageDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

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
    }
}
