using ApiProjectCamp.WebUI.Dtos.ChefDtos;
using ApiProjectCamp.WebUI.Dtos.YummyEventDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ApiProjectCamp.WebUI.Controllers
{
    public class ChefController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ChefController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> ListChefs()
        {
            HttpClient client = _httpClientFactory.CreateClient();
            HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7258/api/Chefs");
            if (responseMessage.IsSuccessStatusCode)
            {
                string jsonData = await responseMessage.Content.ReadAsStringAsync();
                List<ResultChefDto> values = JsonConvert.DeserializeObject<List<ResultChefDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        public IActionResult CreateChef()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateChef(CreateChefDto createChefDto)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            string jsonData = JsonConvert.SerializeObject(createChefDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync("https://localhost:7258/api/Chefs", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("ListChefs");
            }
            return View();
        }

        public async Task<IActionResult> DeleteChef(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            await client.DeleteAsync("https://localhost:7258/api/Chefs?id=" + id);
            return RedirectToAction("ListChefs");
        }

        public async Task<IActionResult> UpdateChef(int id)
        {
            HttpResponseMessage responseMessage = await _httpClientFactory.CreateClient().GetAsync("https://localhost:7258/api/Chefs/GetChefById?id=" + id);
            string jsonData = await responseMessage.Content.ReadAsStringAsync();
            GetByIdChefDto value = JsonConvert.DeserializeObject<GetByIdChefDto>(jsonData);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateChef(UpdateChefDto updateChefDto)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            string jsonData = JsonConvert.SerializeObject(updateChefDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            await client.PutAsync("https://localhost:7258/api/Chefs", stringContent);
            return RedirectToAction("ListChefs");
        }
    }
}
