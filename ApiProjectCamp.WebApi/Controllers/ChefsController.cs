using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChefsController : ControllerBase
    {
        private readonly ApiContext _context;

        public ChefsController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ListChefs()
        {
            List<Chef> chefs = _context.Chefs.ToList();
            return Ok(chefs);
        }

        [HttpPost]
        public IActionResult CreateChef(Chef chef)
        {
            _context.Chefs.Add(chef);
            _context.SaveChanges();
            return Ok("Chef added successfully!");
        }

        [HttpDelete]
        public IActionResult DeleteChef(int id)
        {
            Chef chefToDelete = _context.Chefs.Find(id);
            _context.Chefs.Remove(chefToDelete);
            _context.SaveChanges();
            return Ok("Chef deleted successfully!");
        }

        [HttpGet("GetChefById")]
        public IActionResult GetChefById(int id)
        {
            Chef chef = _context.Chefs.Find(id);
            return Ok(chef);
        }

        [HttpPut]
        public IActionResult UpdateChef(Chef chef)
        {
            _context.Chefs.Update(chef);
            _context.SaveChanges();
            return Ok("Chef updated successfully!");
        }
    }
}
