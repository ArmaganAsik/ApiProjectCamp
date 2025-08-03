using ApiProjectCamp.WebUI.Dtos.ChefDtos;
using ApiProjectCamp.WebUI.Dtos.ReservationDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ApiProjectCamp.WebUI.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ReservationController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> ListReservations()
        {
            HttpClient client = _httpClientFactory.CreateClient();
            HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7258/api/Reservations");
            if (responseMessage.IsSuccessStatusCode)
            {
                string jsonData = await responseMessage.Content.ReadAsStringAsync();
                List<ResultReservationDto> values = JsonConvert.DeserializeObject<List<ResultReservationDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        public IActionResult CreateReservation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation(CreateReservationDto createReservationDto)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            string jsonData = JsonConvert.SerializeObject(createReservationDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync("https://localhost:7258/api/Reservations", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("ListReservations");
            }
            return View();
        }

        public async Task<IActionResult> DeleteReservation(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            await client.DeleteAsync("https://localhost:7258/api/Reservations?id=" + id);
            return RedirectToAction("ListReservations");
        }

        public async Task<IActionResult> UpdateReservation(int id)
        {
            HttpResponseMessage responseMessage = await _httpClientFactory.CreateClient().GetAsync("https://localhost:7258/api/Reservations/GetReservationById?id=" + id);
            string jsonData = await responseMessage.Content.ReadAsStringAsync();
            GetByIdReservationDto value = JsonConvert.DeserializeObject<GetByIdReservationDto>(jsonData);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateReservation(UpdateReservationDto updateReservationDto)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            string jsonData = JsonConvert.SerializeObject(updateReservationDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            await client.PutAsync("https://localhost:7258/api/Reservations", stringContent);
            return RedirectToAction("ListReservations");
        }
    }
}
