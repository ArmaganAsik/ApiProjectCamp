using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Dtos.ContactDtos;
using ApiProjectCamp.WebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ApiContext _context;

        public ContactsController(ApiContext apiContext)
        {
            _context = apiContext;
        }

        [HttpGet]
        public IActionResult ListContacts()
        {
            List<Contact> contacts = _context.Contacts.ToList();
            return Ok(contacts);
        }

        [HttpPost]
        public IActionResult CreateContact(CreateContactDto createContactDto)
        {
            Contact c = new Contact();
            c.MapLocation = createContactDto.MapLocation;
            c.Address = createContactDto.Address;
            c.Phone = createContactDto.Phone;
            c.Mail = createContactDto.Mail;
            c.OpenHours = createContactDto.OpenHours;
            _context.Add(c);
            _context.SaveChanges();
            return Ok("Contact added successfully!");
        }

        [HttpDelete]
        public IActionResult DeleteContacy(int id)
        {
            Contact cToDelete = _context.Contacts.Find(id);
            _context.Contacts.Remove(cToDelete);
            _context.SaveChanges();
            return Ok("Contact deleted successfully!");
        }

        [HttpGet("GetContactById")]
        public IActionResult GetContactById(int id)
        {
            Contact contact = _context.Contacts.Find(id);
            return Ok(contact);
        }

        [HttpPut]
        public IActionResult UpdateContact(UpdateContactDto contact)
        {
            Contact c = new Contact();
            c.ContactId = contact.ContactId;
            c.MapLocation = contact.MapLocation;
            c.Address = contact.Address;
            c.Phone = contact.Phone;
            c.Mail = contact.Mail;
            c.OpenHours = contact.OpenHours;
            _context.Contacts.Update(c);
            _context.SaveChanges();
            return Ok("Contact updated successfully!");
        }
    }
}
