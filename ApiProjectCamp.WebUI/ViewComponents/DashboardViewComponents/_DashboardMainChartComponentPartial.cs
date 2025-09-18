using ApiProjectCamp.WebUI.Dtos.ReservationDtos;
using ApiProjectCamp.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiProjectCamp.WebUI.ViewComponents.DashboardViewComponents
{
    public class _DashboardMainChartComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _DashboardMainChartComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            HttpClient client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7258");
            HttpResponseMessage response = await client.GetAsync("api/Reservations/GetReservationStats");
            string json = await response.Content.ReadAsStringAsync();
            List<ChartReservationDto> data = JsonConvert.DeserializeObject<List<ChartReservationDto>>(json);

            return View(data);

            //var vm = new RevenueChartViewModel
            //{
            //    Labels = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun" },
            //    Income = new List<int> { 5, 15, 14, 36, 32, 32 },
            //    Expense = new List<int> { 7, 11, 30, 18, 25, 13 },
            //    WeeklyEarnings = 675,
            //    MonthlyEarnings = 1587,
            //    YearlyEarnings = 45965,
            //    TotalCustomers = 8257,
            //    TotalIncome = 9857,
            //    ProjectCompleted = 28,
            //    TotalExpense = 6287,
            //    NewCustomers = 684
            //};

            //return View(vm);
        }
    }
}
