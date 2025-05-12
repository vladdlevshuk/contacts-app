using ContactsApp.Data;
using ContactsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
        
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }
        
        [HttpPost]
        public IActionResult Edit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                var existingContact = _context.Contacts.FirstOrDefault(c => c.Id == contact.Id);
                if (existingContact != null)
                {
                    existingContact.Name = contact.Name;
                    existingContact.MobilePhone = contact.MobilePhone;
                    existingContact.JobTitle = contact.JobTitle;
                    existingContact.BirthDate = contact.BirthDate;

                    _context.SaveChanges();
                }
            }
            return Ok();
        }
        
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var contact = _context.Contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            _context.SaveChanges();

            return Ok();
        }
    }
}