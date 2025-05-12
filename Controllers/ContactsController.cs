using ContactsApp.Data;
using ContactsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 13)
        {
            try
            {
                var totalContacts = await _context.Contacts.CountAsync();
                var contacts = await _context.Contacts
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var viewModel = new ContactListView
                {
                    Contacts = contacts,
                    TotalCount = totalContacts,
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalContacts / (double)pageSize)
                };

                return View(viewModel);
            }
            catch
            {
                return StatusCode(500, "Ошибка при загрузке списка контактов.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Contact contact)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return StatusCode(500, "Ошибка при создании контакта.");
            }
        }

        [HttpPost]
        public IActionResult Edit(Contact contact)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existingContact = _context.Contacts.FirstOrDefault(c => c.Id == contact.Id);
                if (existingContact == null)
                    return NotFound();

                existingContact.Name = contact.Name;
                existingContact.MobilePhone = contact.MobilePhone;
                existingContact.JobTitle = contact.JobTitle;
                existingContact.BirthDate = contact.BirthDate;

                _context.SaveChanges();
                return Ok();
            }
            catch
            {
                return StatusCode(500, "Ошибка при обновлении контакта.");
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var contact = _context.Contacts.FirstOrDefault(c => c.Id == id);
                if (contact == null)
                    return NotFound();

                _context.Contacts.Remove(contact);
                _context.SaveChanges();
                return Ok();
            }
            catch
            {
                return StatusCode(500, "Ошибка при удалении контакта.");
            }
        }
    }
}