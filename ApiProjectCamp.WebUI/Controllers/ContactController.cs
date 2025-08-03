using ApiProjectCamp.WebUI.Dtos.ContactDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace ApiProjectCamp.WebUI.Controllers
{
    public class ContactController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ContactController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> ListContacts()
        {
            HttpClient client = _httpClientFactory.CreateClient();
            HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7258/api/Contacts");
            if (responseMessage.IsSuccessStatusCode)
            {
                string jsonData = await responseMessage.Content.ReadAsStringAsync();
                List<ResultContactDto> values = JsonConvert.DeserializeObject<List<ResultContactDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        public IActionResult CreateContact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact(CreateContactDto createContactDto)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            string jsonData = JsonConvert.SerializeObject(createContactDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync("https://localhost:7258/api/Contacts", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("ListContacts");
            }
            return View();
        }

        public async Task<IActionResult> DeleteContact(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            await client.DeleteAsync("https://localhost:7258/api/Contacts?id=" + id);
            return RedirectToAction("ListContacts");
        }

        public async Task<IActionResult> UpdateContact(int id)
        {
            HttpResponseMessage responseMessage = await _httpClientFactory.CreateClient().GetAsync("https://localhost:7258/api/Contacts/GetContactById?id=" + id);
            string jsonData = await responseMessage.Content.ReadAsStringAsync();
            GetByIdContactDto value = JsonConvert.DeserializeObject<GetByIdContactDto>(jsonData);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateContact(UpdateContactDto updateContactDto)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            string jsonData = JsonConvert.SerializeObject(updateContactDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            await client.PutAsync("https://localhost:7258/api/Contacts", stringContent);
            return RedirectToAction("ListContacts");
        }
    }
}
