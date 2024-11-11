using ContactApp.JsonRepository;
using ContactApp.JsonRepository.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ContactRepository _contactRepository = new ContactRepository();

        [HttpGet("GetAllContacts")]
        public IActionResult GetAllContacts() => Ok(_contactRepository.GetAllContacts());

        [HttpPost]
        public IActionResult CreateContact([FromBody] Contact contact)
        {
            var contacts = _contactRepository.GetAllContacts();
            contact.Id = contacts.Count > 0 ? contacts.Max(c => c.Id) + 1 : 1;
            contacts.Add(contact);
            _contactRepository.SaveAllContacts(contacts);
            return CreatedAtAction(nameof(GetAllContacts), new { id = contact.Id }, contact);
        }

       
        [HttpPut("{id}")]
        public IActionResult UpdateContact(int id, [FromBody] Contact updatedContact)
        {
            var contacts = _contactRepository.GetAllContacts();
            var contact = contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null) return NotFound();

            contact.FirstName = updatedContact.FirstName;
            contact.LastName = updatedContact.LastName;
            contact.Email = updatedContact.Email;

            _contactRepository.SaveAllContacts(contacts);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteContact(int id)
        {
            var contacts = _contactRepository.GetAllContacts();
            var contact = contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null) return NotFound();

            contacts.Remove(contact);
            _contactRepository.SaveAllContacts(contacts);
            return NoContent();
        }

        [HttpGet]
        public IActionResult GetContacts(string search = null, string sortBy = "FirstName", int page = 1, int pageSize = 10)
        {
            var contacts = _contactRepository.GetAllContacts();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                contacts = contacts.Where(c =>
                    c.FirstName.ToLower().Contains(search) ||
                    c.LastName.ToLower().Contains(search) ||
                    c.Email.ToLower().Contains(search)).ToList();
            }

            // Sorting
            contacts = sortBy.ToLower() switch
            {
                "firstname" => contacts.OrderBy(c => c.FirstName).ToList(),
                "lastname" => contacts.OrderBy(c => c.LastName).ToList(),
                _ => contacts.OrderBy(c => c.FirstName).ToList()
            };

            // Pagination
            var totalItems = contacts.Count;
            contacts = contacts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(new { totalItems, contacts });
        }

    }
}
