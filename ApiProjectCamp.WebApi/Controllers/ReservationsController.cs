using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Dtos.ReservationDtos;
using ApiProjectCamp.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ApiContext _apiContext;
        private readonly IMapper _mapper;

        public ReservationsController(ApiContext apiContext, IMapper mapper)
        {
            _apiContext = apiContext;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult ListReservations()
        {
            List<Reservation> reservations = _apiContext.Reservations.ToList();
            List<ResultReservationDto> resultReservationDtos = _mapper.Map<List<ResultReservationDto>>(reservations);
            return Ok(resultReservationDtos);
        }

        [HttpPost]
        public IActionResult CreateReservation(CreateReservationDto createReservationDto)
        {
            Reservation reservation = _mapper.Map<Reservation>(createReservationDto);
            _apiContext.Reservations.Add(reservation);
            _apiContext.SaveChanges();
            return Ok("Reservation added successfully!");
        }

        [HttpDelete]
        public IActionResult DeleteReservation(int id)
        {
            Reservation reservation = _apiContext.Reservations.Find(id);
            _apiContext.Reservations.Remove(reservation);
            _apiContext.SaveChanges();
            return Ok("Reservation deleted successfully!");
        }

        [HttpPut]
        public IActionResult UpdateReservation(UpdateReservationDto updateReservationDto)
        {
            _apiContext.Reservations.Update(_mapper.Map<Reservation>(updateReservationDto));
            _apiContext.SaveChanges();
            return Ok("Reservation updated successfully!");
        }

        [HttpGet("GetReservationById")]
        public IActionResult GetReservationById(int id)
        {
            Reservation reservation = _apiContext.Reservations.Find(id);
            GetByIdReservationDto getByIdReservationDto = _mapper.Map<GetByIdReservationDto>(reservation);
            return Ok(getByIdReservationDto);
        }
    }
}
