using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Dtos.GroupReservationDtos;
using ApiProjectCamp.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupReservationsController : ControllerBase
    {
        private readonly ApiContext _apiContext;
        private readonly IMapper _mapper;

        public GroupReservationsController(ApiContext apiContext, IMapper mapper)
        {
            _apiContext = apiContext;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult ListGroupReservations()
        {
            List<GroupReservation> groupReservations = _apiContext.GroupReservations.ToList();
            return Ok(groupReservations);
        }

        [HttpPost]
        public IActionResult CreateGroupReservation(CreateGroupReservationDto createGroupReservationDto)
        {
            GroupReservation groupReservation = _mapper.Map<GroupReservation>(createGroupReservationDto);
            _apiContext.GroupReservations.Add(groupReservation);
            _apiContext.SaveChanges();
            return Ok("Group Reservation added successfully!");
        }

        [HttpDelete]
        public IActionResult DeleteGroupReservation(int id)
        {
            GroupReservation groupReservationtToDelete = _apiContext.GroupReservations.Find(id);
            _apiContext.GroupReservations.Remove(groupReservationtToDelete);
            _apiContext.SaveChanges();
            return Ok("Group Reservation deleted successfully!");
        }

        [HttpGet("GetGroupReservationById")]
        public IActionResult GetGroupReservationById(int id)
        {
            GroupReservation groupReservation = _apiContext.GroupReservations.Find(id);
            ResultGroupReservationDto resultGroupReservationDto = _mapper.Map<ResultGroupReservationDto>(groupReservation);
            return Ok(resultGroupReservationDto);
        }

        [HttpPut]
        public IActionResult UpdateGroupReservation(UpdateGroupReservationDto updateGroupReservationDto)
        {
            GroupReservation groupReservation = _mapper.Map<GroupReservation>(updateGroupReservationDto);
            _apiContext.GroupReservations.Update(groupReservation);
            _apiContext.SaveChanges();
            return Ok("Group Reservation updated successfully!");
        }
    }
}
