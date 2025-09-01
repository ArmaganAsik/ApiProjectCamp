using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebUI.ViewComponents.DashboardViewComponents
{
    public class _DashboardWidgetsComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _DashboardWidgetsComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int r1, r2, r3, r4;
            Random rnd = new Random();
            r1 = rnd.Next(1, 35);
            r2 = rnd.Next(1, 35);
            r3 = rnd.Next(1, 35);
            r4 = rnd.Next(1, 35);

            HttpClient client = _httpClientFactory.CreateClient();
            HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7258/api/Reservations/GetTotalReservationCount");
            string jsonData = await responseMessage.Content.ReadAsStringAsync();
            ViewBag.TotResCount = jsonData;
            ViewBag.R1 = r1;

            HttpClient client2 = _httpClientFactory.CreateClient();
            HttpResponseMessage responseMessage2 = await client2.GetAsync("https://localhost:7258/api/Reservations/GetTotalCustomerCount");
            string jsonData2 = await responseMessage2.Content.ReadAsStringAsync();
            ViewBag.TotCusCount = jsonData2;
            ViewBag.R2 = r2;

            HttpClient client3 = _httpClientFactory.CreateClient();
            HttpResponseMessage responseMessage3 = await client2.GetAsync("https://localhost:7258/api/Reservations/GetPendingReservations");
            string jsonData3 = await responseMessage3.Content.ReadAsStringAsync();
            ViewBag.PenResCount = jsonData3;
            ViewBag.R3 = r3;

            HttpClient client4 = _httpClientFactory.CreateClient();
            HttpResponseMessage responseMessage4 = await client2.GetAsync("https://localhost:7258/api/Reservations/GetApprovedReservations");
            string jsonData4 = await responseMessage4.Content.ReadAsStringAsync();
            ViewBag.AppResCount = jsonData4;
            ViewBag.R4 = r4;

            return View();
        }
    }
}
