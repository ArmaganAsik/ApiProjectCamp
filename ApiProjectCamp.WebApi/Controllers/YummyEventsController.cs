using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YummyEventsController : ControllerBase
    {
        private readonly ApiContext _apiContext;

        public YummyEventsController(ApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        [HttpGet]
        public IActionResult ListYummyEvents()
        {
            List<YummyEvent> yummyEvents = _apiContext.YummyEvents.ToList();
            return Ok(yummyEvents);
        }

        [HttpPost]
        public IActionResult CreateYummyEvent(YummyEvent yummyEvent)
        {
            _apiContext.YummyEvents.Add(yummyEvent);
            _apiContext.SaveChanges();
            return Ok("Yummy Event added successfully!");
        }

        [HttpDelete]
        public IActionResult DeleteYummyEvent(int id)
        {
            YummyEvent yummyEventToDelete = _apiContext.YummyEvents.Find(id);
            _apiContext.YummyEvents.Remove(yummyEventToDelete);
            _apiContext.SaveChanges();
            return Ok("Yummy Event deleted successfully!");
        }

        [HttpPut]
        public IActionResult UpdateYummyEvent(YummyEvent yummyEvent)
        {
            _apiContext.YummyEvents.Update(yummyEvent);
            return Ok("Yummy event updated successfully!");
        }

        [HttpGet("GetYummyEventById")]
        public IActionResult GetYummyEventById(int id)
        {
            YummyEvent yummyEvent = _apiContext.YummyEvents.Find(id);
            return Ok(yummyEvent);
        }
    }
}
