using ApiProjectCamp.WebUI.Dtos.AISuggestionDtos;
using ApiProjectCamp.WebUI.Dtos.ProductDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ApiProjectCamp.WebUI.ViewComponents.DashboardViewComponents
{
    public class _DashboardAIDailyMenuSuggestionComponentPartial : ViewComponent
    {
        private const string OPENAI_API_KEY = "";

        private readonly IHttpClientFactory _httpClientFactory;

        public _DashboardAIDailyMenuSuggestionComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var openAiClient = _httpClientFactory.CreateClient();
            openAiClient.BaseAddress = new Uri("https://api.openai.com/");
            openAiClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", OPENAI_API_KEY);

            string prompt = @"
Aşağıdaki formatta rastgele 4 farklı dünya mutfağından her biri için günlük bir menü oluştur. Burada ülkeler aşağıda verilecektir.

ÖNEMLİ KURALLAR:
- Mutlaka 4 FARKLI ülke mutfağı seç.
- Daha önce seçtiğin mutfakları tekrar etme (iç mantığında çeşitlilik üret).
- Seçim yapılacak ülkeler : Türkiye, Almanya, Fransa, İtalya, İspanya, Portekiz, Bulgaristan, Gürcistan, Yunanistan, İran, Çin.
- Ülkeleri HER SEFERİNDE FARKLI seç.
- Tüm içerik TÜRKÇE olacak.
- Ülke adını Türkçe yaz (ör: “İtalya Mutfağı”).
- ISO Country Code zorunlu (ör: IT, TR, ES, GR, BG vb.)
- Örnek vermiyorum, tamamen özgün üret.
- Cevap sadece geçerli JSON olsun.

JSON formatı:
[
  {
    ""Cuisine"": ""X Mutfağı"",
    ""CountryCode"": ""XX"",
    ""MenuTitle"": ""Günlük Menü"",
    ""Items"": [
      { ""Name"": ""Yemek 1"", ""Description"": ""Açıklama"", ""Price"": 100 },
      { ""Name"": ""Yemek 2"", ""Description"": ""Açıklama"", ""Price"": 120 },
      { ""Name"": ""Yemek 3"", ""Description"": ""Açıklama"", ""Price"": 90 },
      { ""Name"": ""Yemek 4"", ""Description"": ""Açıklama"", ""Price"": 70 }
    ]
  }
]
";

            var body = new
            {
                model = "gpt-4.1-mini",   // istersen değiştir
                messages = new[]
                {
                new { role = "system", content = "Sadece JSON üret." },
                new { role = "user", content = prompt }
            }
            };

            var jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await openAiClient.PostAsync("v1/chat/completions", content);
            var responseJson = await response.Content.ReadAsStringAsync();

            dynamic obj = JsonConvert.DeserializeObject(responseJson);
            string aiContent = obj.choices[0].message.content.ToString();

            List<MenuSuggestionDto> menus;

            try
            {
                menus = JsonConvert.DeserializeObject<List<MenuSuggestionDto>>(aiContent);
            }
            catch
            {
                menus = new();
            }

            return View(menus);
        }
    }
}