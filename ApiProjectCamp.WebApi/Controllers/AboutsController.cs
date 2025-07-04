using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Dtos.AboutDtos;
using ApiProjectCamp.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutsController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public AboutsController(ApiContext contex, IMapper mapper)
        {
            _context = contex;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult ListAbouts()
        {
            List<About> abouts = _context.Abouts.ToList();
            return Ok(abouts);
        }

        [HttpPost]
        public IActionResult CreateAbout(CreateAboutDto createAboutDto)
        {
            About about = _mapper.Map<About>(createAboutDto);
            _context.Abouts.Add(about);
            _context.SaveChanges();
            return Ok("ABOUT added successfully!");
        }

        [HttpDelete]
        public IActionResult DeleteAbout(int id)
        {
            About aboutToDelete = _context.Abouts.Find(id);
            _context.Abouts.Remove(aboutToDelete);
            _context.SaveChanges();
            return Ok("ABOUT deleted successfully!");
        }

        [HttpGet("GetAboutById")]
        public IActionResult GetAboutById(int id)
        {
            About about = _context.Abouts.Find(id);
            return Ok(about);
        }

        [HttpPut]
        public IActionResult UpdateAbout(UpdateAboutDto updateAboutDto)
        {
            About about = _mapper.Map<About>(updateAboutDto);
            _context.Abouts.Update(about);
            _context.SaveChanges();
            return Ok("ABOUT updated successfully!");
        }
    }
}
