using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Dtos.CategoryDtos;
using ApiProjectCamp.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ApiProjectCamp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(ApiContext contex, IMapper mapper)
        {
            _context = contex;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult ListCategories()
        {
            List<Category> categories = _context.Categories.ToList();
            return Ok(categories);
        }

        [HttpPost]
        public IActionResult CreateCategory(CreateCategoryDto createCategoryDto)
        {
            Category category = _mapper.Map<Category>(createCategoryDto);
            _context.Categories.Add(category);
            _context.SaveChanges();
            return Ok("Category added successfully!");
        }

        [HttpDelete]
        public IActionResult DeleteCategory(int id)
        {
            Category categoryToDelete = _context.Categories.Find(id);
            _context.Categories.Remove(categoryToDelete);
            _context.SaveChanges();
            return Ok("Category deleted successfully!");
        }

        [HttpGet("GetCategoryById")]
        public IActionResult GetCategoryById(int id)
        {
            Category category = _context.Categories.Find(id);
            return Ok(category);
        }

        [HttpPut]
        public IActionResult UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            Category category = _mapper.Map<Category>(updateCategoryDto);
            _context.Categories.Update(category);
            _context.SaveChanges();
            return Ok("Category updated successfully!");
        }
    }
}
