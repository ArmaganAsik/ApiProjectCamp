using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebUI.ViewComponents.HomePageViewComponents
{
    public class _HomePageStatisticsComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _HomePageStatisticsComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            HttpClient client1 = _httpClientFactory.CreateClient();
            HttpResponseMessage responseMessage1 = await client1.GetAsync("https://localhost:7258/api/Statistics/GetProductCount");
            string jsonData1 = await responseMessage1.Content.ReadAsStringAsync();
            ViewBag.TotProdCount = jsonData1;

            HttpClient client2 = _httpClientFactory.CreateClient();
            HttpResponseMessage responseMessage2 = await client2.GetAsync("https://localhost:7258/api/Statistics/GetReservationCount");
            string jsonData2 = await responseMessage2.Content.ReadAsStringAsync();
            ViewBag.TotResCount = jsonData2;

            HttpClient client3 = _httpClientFactory.CreateClient();
            HttpResponseMessage responseMessage3 = await client3.GetAsync("https://localhost:7258/api/Statistics/GetChefCount");
            string jsonData3 = await responseMessage3.Content.ReadAsStringAsync();
            ViewBag.TotChefCount = jsonData3;

            HttpClient client4 = _httpClientFactory.CreateClient();
            HttpResponseMessage responseMessage4 = await client4.GetAsync("https://localhost:7258/api/Statistics/GetTotalGuestCount");
            string jsonData4 = await responseMessage4.Content.ReadAsStringAsync();
            ViewBag.TotGuestCount = jsonData4;

            return View();
        }
    }
}
