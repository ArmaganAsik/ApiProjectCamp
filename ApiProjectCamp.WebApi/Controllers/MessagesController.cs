using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Dtos.MessageDtos;
using ApiProjectCamp.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApiContext _context;

        public MessagesController(IMapper mapper, ApiContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public IActionResult ListMessages()
        {
            List<Message> messages = _context.Messages.ToList();
            return Ok(_mapper.Map<List<ResultMessageDto>>(messages));
        }

        [HttpPost]
        public IActionResult CreateMessage(CreateMessageDto createMessageDto)
        {
            Message m = _mapper.Map<Message>(createMessageDto);
            _context.Messages.Add(m);
            _context.SaveChanges();
            return Ok("Message added successfully!");
        }

        [HttpDelete]
        public IActionResult DeleteMessage(int id)
        {
            _context.Messages.Remove(_context.Messages.Find(id));
            _context.SaveChanges();
            return Ok("Message deleted successfully!");
        }

        [HttpGet("GetMessageById")]
        public IActionResult GetMessageById(int id)
        {
            Message m = _context.Messages.Find(id);
            return Ok(_mapper.Map<GetByIdMessageDto>(m));
        }

        [HttpPut]
        public IActionResult UpdateMessage(UpdateMessageDto updateMessageDto)
        {
            _context.Messages.Update(_mapper.Map<Message>(updateMessageDto));
            _context.SaveChanges();
            return Ok("Message updated successfully!");
        }

        [HttpGet("MessageListByIsReadFalse")]
        public IActionResult MessageListByIsReadFalse()
        {
            List<Message> values = _context.Messages.Where(x => x.IsRead == false).ToList();
            return Ok(values);
        }
    }
}
