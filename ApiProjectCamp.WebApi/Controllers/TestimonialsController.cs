using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestimonialsController : ControllerBase
    {
        private readonly ApiContext _apiContext;

        public TestimonialsController(ApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        [HttpGet]
        public IActionResult ListTestimonials()
        {
            List<Testimonial> testimonials = _apiContext.Testimonials.ToList();
            return Ok(testimonials);
        }

        [HttpPost]
        public IActionResult CreateTestimonail(Testimonial testimonial)
        {
            _apiContext.Testimonials.Add(testimonial);
            _apiContext.SaveChanges();
            return Ok("Testimonial added successfully!");
        }

        [HttpDelete]
        public IActionResult DeleteTestimonial(int id)
        {
            Testimonial testimonialToDelete = _apiContext.Testimonials.Find(id);
            _apiContext.Testimonials.Remove(testimonialToDelete);
            _apiContext.SaveChanges();
            return Ok("Testimonial deleted successfully!");
        }

        [HttpPut]
        public IActionResult UpdateTestimonial(Testimonial testimonial)
        {
            _apiContext.Update(testimonial);
            _apiContext.SaveChanges();
            return Ok("Testimonial updated successfully!");
        }

        [HttpGet("GetTestimonialById")]
        public IActionResult GetTestimonialById(int id)
        {
            Testimonial testimonial = _apiContext.Testimonials.Find(id);
            return Ok(testimonial);
        }
    }
}
