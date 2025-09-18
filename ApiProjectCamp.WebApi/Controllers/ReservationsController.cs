using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Dtos.ReservationDtos;
using ApiProjectCamp.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO.Pipes;

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

        [HttpGet("GetTotalReservationCount")]
        public IActionResult GetTotalReservationCount()
        {
            int value = _apiContext.Reservations.Count();
            return Ok(value);
        }

        [HttpGet("GetTotalCustomerCount")]
        public IActionResult GetTotalCustomerCount()
        {
            int value = _apiContext.Reservations.Sum(x => x.NumberOfPeople);
            return Ok(value);
        }

        [HttpGet("GetPendingReservations")]
        public IActionResult GetPendingReservations()
        {
            int value = _apiContext.Reservations.Where(x => x.ReservationStatus == "Onay Bekliyor").Count();
            return Ok(value);
        }

        [HttpGet("GetApprovedReservations")]
        public IActionResult GetApprovedReservations()
        {
            int value = _apiContext.Reservations.Where(x => x.ReservationStatus == "Onaylandı").Count();
            return Ok(value);
        }

        [HttpGet("GetReservationStats")]
        public IActionResult GetReservationStats()
        {
            DateTime fourMonthsAgo = DateTime.Today.AddMonths(-3);

            var rawData = _apiContext.Reservations
                .Where(x => x.ReservationDate >= fourMonthsAgo)
                .GroupBy(x => new { x.ReservationDate.Month, x.ReservationDate.Year })
                .Select(t => new
                {
                    t.Key.Year,
                    t.Key.Month,
                    Approved = t.Count(x => x.ReservationStatus == "Onaylandı"),
                    Pending = t.Count(x => x.ReservationStatus == "Onay Bekliyor"),
                    Canceled = t.Count(x => x.ReservationStatus == "İptal Edildi"),
                })
                .OrderBy(p => p.Month).ThenBy(p => p.Month)
                .ToList();

            var result = rawData.Select(x => new ChartReservationDto
            {
                Month = new DateTime(x.Year, x.Month, 1).ToString("MMM yyyy"),
                Approved = x.Approved,
                Pending = x.Pending,
                Canceled = x.Canceled
            }).ToList();

            return Ok(result);
        }
    }
}
