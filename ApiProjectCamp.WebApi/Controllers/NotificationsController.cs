using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Dtos.NotificationDtos;
using ApiProjectCamp.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApiContext _context;

        public NotificationsController(IMapper mapper, ApiContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public IActionResult ListNotifications()
        {
            List<Notification> Notifications = _context.Notifications.ToList();
            return Ok(_mapper.Map<List<ResultNotificationDto>>(Notifications));
        }

        [HttpPost]
        public IActionResult CreateNotification(CreateNotificationDto createNotificationDto)
        {
            Notification n = _mapper.Map<Notification>(createNotificationDto);
            _context.Add(n);
            _context.SaveChanges();
            return Ok("Notification added successfully!");
        }

        [HttpDelete]
        public IActionResult DeleteNotification(int id)
        {
            _context.Notifications.Remove(_context.Notifications.Find(id));
            _context.SaveChanges();
            return Ok("Notification deleted successfully!");
        }

        [HttpGet("GetNotificationById")]
        public IActionResult GetNotificationById(int id)
        {
            Notification n = _context.Notifications.Find(id);
            return Ok(_mapper.Map<GetByIdNotificationDto>(n));
        }

        [HttpPut]
        public IActionResult UpdateNotification(UpdateNotificationDto updateNotificationDto)
        {
            _context.Notifications.Update(_mapper.Map<Notification>(updateNotificationDto));
            _context.SaveChanges();
            return Ok("Notification updated successfully!");
        }
    }
}
