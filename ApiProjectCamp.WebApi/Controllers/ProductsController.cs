using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Dtos.ProductDtos;
using ApiProjectCamp.WebApi.Entities;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ApiProjectCamp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IValidator<Product> _validator;
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public ProductsController(IValidator<Product> validator, ApiContext context, IMapper mapper)
        {
            _validator = validator;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult ListProducts()
        {
            List<Product> products = _context.Products.ToList();
            return Ok(products);
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            ValidationResult validationResult = _validator.Validate(product);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
            }
            else
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return Ok(new { message = "Product added successfully!", data = product });
            }
        }

        [HttpDelete]
        public IActionResult DeleteProduct(int id)
        {
            Product pToDelete = _context.Products.Find(id);
            _context.Products.Remove(pToDelete);
            _context.SaveChanges();
            return Ok("Product deleted successfully!");
        }

        [HttpGet("GetProductById")]
        public IActionResult GetProductById(int id)
        {
            Product p = _context.Products.Find(id);
            return Ok(p);
        }

        [HttpPut]
        public IActionResult UpdateProduct(Product p)
        {
            //Product pToUpdate = _context.Products.Find(p.ProductId);
            //pToUpdate.Name = p.Name;
            //pToUpdate.Description = p.Description;
            //pToUpdate.Price = p.Price;
            //pToUpdate.ImageUrl = p.ImageUrl;
            //_context.SaveChanges();
            //return Ok("Product updated successfully!");
            ValidationResult validationResult = _validator.Validate(p);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
            }
            else
            {
                _context.Products.Update(p);
                _context.SaveChanges();
                return Ok(new { message = "Product updated successfully!", data = p });
            }
        }

        [HttpPost("CreateProductWithCategory")]
        public IActionResult CreateProductWithCategory(CreateProductDto createProductDto)
        {
            Product product = _mapper.Map<Product>(createProductDto);
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok("Product with category added successfully!");
        }

        [HttpGet("ListProductsWithCategories")]
        public IActionResult ListProductsWithCategories()
        {
            List<Product> products = _context.Products.Include(x => x.Category).ToList();
            return Ok(_mapper.Map<List<ResultProductWithCategoryDto>>(products));
        }

        [HttpPut("UpdateProductWithCategory")]
        public IActionResult UpdateProductWithCategory(UpdateProductWithCategoryDto updateProductWithCategoryDto)
        {
            Product p = _mapper.Map<Product>(updateProductWithCategoryDto);
            _context.Products.Update(p);
            _context.SaveChanges();
            return Ok("Product with category updated successfully!");
        }
    }
}
