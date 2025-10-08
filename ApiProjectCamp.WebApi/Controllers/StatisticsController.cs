using ApiProjectCamp.WebApi.Context;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly ApiContext _context;

        public StatisticsController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet("GetProductCount")]
        public IActionResult GetProductCount()
        {
            int value = _context.Products.Count();
            return Ok(value);
        }
        [HttpGet("GetReservationCount")]
        public IActionResult GetReservationCount()
        {
            int value = _context.Reservations.Count();
            return Ok(value);
        }
        [HttpGet("GetChefCount")]
        public IActionResult GetChefCount()
        {
            int value = _context.Chefs.Count();
            return Ok(value);
        }
        [HttpGet("GetTotalGuestCount")]
        public IActionResult GetTotalGuestCount()
        {
            int value = _context.Reservations.Sum(x => x.NumberOfPeople);
            return Ok(value);
        }
    }
}
