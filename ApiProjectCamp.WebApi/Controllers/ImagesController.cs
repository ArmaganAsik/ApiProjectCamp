using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Dtos.CategoryDtos;
using ApiProjectCamp.WebApi.Dtos.ImageDtos;
using ApiProjectCamp.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public ImagesController(ApiContext contex, IMapper mapper)
        {
            _context = contex;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult ListImages()
        {
            List<Image> images = _context.Images.ToList();
            List<ResultImageDto> resultImageDtos = _mapper.Map<List<ResultImageDto>>(images);
            return Ok(resultImageDtos);
        }

        [HttpPost]
        public IActionResult CreateImage(CreateImageDto createImageDto)
        {
            Image image = _mapper.Map<Image>(createImageDto);
            _context.Images.Add(image);
            _context.SaveChanges();
            return Ok("Image added successfully!");
        }

        [HttpDelete]
        public IActionResult DeleteImage(int id)
        {
            Image imageToDelete = _context.Images.Find(id);
            _context.Images.Remove(imageToDelete);
            _context.SaveChanges();
            return Ok("Image deleted successfully!");
        }

        [HttpGet("GetImageById")]
        public IActionResult GetImageById(int id)
        {
            Image image = _context.Images.Find(id);
            GetByIdImageDto getByIdImageDto = _mapper.Map<GetByIdImageDto>(image);
            return Ok(getByIdImageDto);
        }

        [HttpPut]
        public IActionResult UpdateImage(UpdateImageDto updateImageDto)
        {
            Image image = _mapper.Map<Image>(updateImageDto);
            _context.Images.Update(image);
            _context.SaveChanges();
            return Ok("Image updated successfully!");
        }
    }
}
