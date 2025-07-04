using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiProjectCamp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly ApiContext _apiContext;

        public ServicesController(ApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        [HttpGet]
        public IActionResult ListServices()
        {
            List<Service> services = _apiContext.Services.ToList();
            return Ok(services);
        }

        [HttpPost]
        public IActionResult CreateService(Service service)
        {
            _apiContext.Services.Add(service);
            _apiContext.SaveChanges();
            return Ok("Service added successfully!");
        }

        [HttpDelete]
        public IActionResult DeleteService(int id)
        {
            Service serviceToDelete = _apiContext.Services.Find(id);
            _apiContext.Services.Remove(serviceToDelete);
            _apiContext.SaveChanges();
            return Ok("Service deleted successfully!");
        }

        [HttpPut]
        public IActionResult UpdateService(Service service)
        {
            Service serviceToUpdate = _apiContext.Services.Find(service.ServiceId);
            serviceToUpdate.Title = service.Title;
            serviceToUpdate.Description = service.Description;
            serviceToUpdate.IconUrl = service.IconUrl;
            _apiContext.SaveChanges();
            return Ok("Service updated successfully!");
        }

        [HttpGet("GetServiceById")]
        public IActionResult GetServiceById(int id)
        {
            return Ok(_apiContext.Services.Find(id));
        }
    }
}
