using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Dtos.FeatureDtos;
using ApiProjectCamp.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApiContext _context;

        public FeaturesController(IMapper mapper, ApiContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public IActionResult ListFeatures()
        {
            List<Feature> features = _context.Features.ToList();
            return Ok(_mapper.Map<List<ResultFeatureDto>>(features));
        }

        [HttpPost]
        public IActionResult CreateFeature(CreateFeatureDto createFeatureDto)
        {
            Feature f = _mapper.Map<Feature>(createFeatureDto);
            _context.Add(f);
            _context.SaveChanges();
            return Ok("Feature added successfully!");
        }

        [HttpDelete]
        public IActionResult DeleteFeature(int id)
        {
            _context.Features.Remove(_context.Features.Find(id));
            _context.SaveChanges();
            return Ok("Feature deleted successfully!");
        }

        [HttpGet("GetFeatureById")]
        public IActionResult GetFeatureById(int id)
        {
            Feature f = _context.Features.Find(id);
            return Ok(_mapper.Map<GetByIdFeatureDto>(f));
        }

        [HttpPut]
        public IActionResult UpdateFeature(UpdateFeatureDto updateFeatureDto)
        {
            _context.Features.Update(_mapper.Map<Feature>(updateFeatureDto));
            _context.SaveChanges();
            return Ok("Feature updated successfully!");
        }
    }
}
