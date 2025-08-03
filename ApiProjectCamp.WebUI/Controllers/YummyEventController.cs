using ApiProjectCamp.WebUI.Dtos.YummyEventDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace ApiProjectCamp.WebUI.Controllers
{
    public class YummyEventController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public YummyEventController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> ListYummyEvents()
        {
            HttpClient client = _httpClientFactory.CreateClient();
            HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7258/api/YummyEvents");
            if (responseMessage.IsSuccessStatusCode)
            {
                string jsonData = await responseMessage.Content.ReadAsStringAsync();
                List<ResultYummyEventDto> values = JsonConvert.DeserializeObject<List<ResultYummyEventDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        public IActionResult CreateYummyEvent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateYummyEvent(CreateYummyEventDto createYummyEventDto)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            string jsonData = JsonConvert.SerializeObject(createYummyEventDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync("https://localhost:7258/api/YummyEvents", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("ListYummyEvents");
            }
            return View();
        }

        public async Task<IActionResult> DeleteYummyEvent(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            await client.DeleteAsync("https://localhost:7258/api/YummyEvents?id=" + id);
            return RedirectToAction("ListYummyEvents");
        }

        public async Task<IActionResult> UpdateYummyEvent(int id)
        {
            HttpResponseMessage responseMessage = await _httpClientFactory.CreateClient().GetAsync("https://localhost:7258/api/YummyEvents/GetYummyEventById?id=" + id);
            string jsonData = await responseMessage.Content.ReadAsStringAsync();
            GetByIdYummyEventDto value = JsonConvert.DeserializeObject<GetByIdYummyEventDto>(jsonData);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateYummyEvent(UpdateYummyEventDto updateYummyEventDto)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            string jsonData = JsonConvert.SerializeObject(updateYummyEventDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            await client.PutAsync("https://localhost:7258/api/YummyEvents", stringContent);
            return RedirectToAction("ListYummyEvents");
        }
    }
}
