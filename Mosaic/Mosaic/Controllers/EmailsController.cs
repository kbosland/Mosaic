using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mosaic.Models;
using Microsoft.AspNetCore.Http;
using Mosaic.Services;


namespace Mosaic.Controllers
{
    public class EmailsController : Controller
    {
        private readonly MosaicContext _context;
        private readonly IEmailAuthentication _service;

        public EmailsController(MosaicContext context, IEmailAuthentication service)
        {
            _context = context;
            _service = service;
        }

        public IActionResult Menu ()
        {
            if (HttpContext.Session.GetInt32("type") == 0)
            {
                return RedirectToAction("EmailMenu", "Students");
            } else if (HttpContext.Session.GetInt32("type") == 1)
            {
                return RedirectToAction("EmailMenu", "Professors");
            }

            return RedirectToAction("Home", "Students");
        }

        public IActionResult Reply (string subject, string sender)
        {
            HttpContext.Session.SetString("subject", subject);
            HttpContext.Session.SetString("receiver", sender);
            return RedirectToAction("Create");
        }

        // GET: Emails/Inbox
        public IActionResult Inbox()
        {
            string username = HttpContext.Session.GetString("username");
            var emails = _context.Email.ToList();
            List<Email> inbox = new List<Email>();
            for (int i = 0; i < emails.Count; i++)
            {
                if (emails[i].Receiver.Equals(username))
                {
                    inbox.Add(emails[i]);
                }
            }
            ViewData["Inbox"] = inbox;
            ViewData["Username"] = username;
            return View();
        }

        // GET: Emails
        public async Task<IActionResult> Index()
        {
            return View(await _context.Email.ToListAsync());
        }

        // GET: Emails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var email = await _context.Email
                .SingleOrDefaultAsync(m => m.Id == id);
            if (email == null)
            {
                return NotFound();
            }

            return View(email);
        }

        // GET: Emails/Create
        public IActionResult Create()
        {
            ViewData["receiver"] = HttpContext.Session.GetString("receiver");
            ViewData["sender"] = HttpContext.Session.GetString("username");
            ViewData["subject"] = HttpContext.Session.GetString("subject");
            ViewData["message"] = HttpContext.Session.GetString("message");
            return View();
        }

        // POST: Emails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string sender, string receiver, string subject, string message)
        {
            Email email = new Email { Sender = sender, Receiver = receiver, Subject = subject, Message = message };

            if (ModelState.IsValid)
            {
                email.Status = "Delivered @ " + DateTime.Now.ToString("HH:mm") + " on " + DateTime.Today.ToString("dd-MM-yyyy");
                _context.Add(email);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("receiver", "");
                HttpContext.Session.SetString("subject", "");
                HttpContext.Session.SetString("message", "");
                ModelState.Clear();
                return View();
            }
            return View(email);
        }

        // GET: Emails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var email = await _context.Email.SingleOrDefaultAsync(m => m.Id == id);
            if (email == null)
            {
                return NotFound();
            }
            return View(email);
        }

        // POST: Emails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Sender,Receiever,Subject,Message,Status")] Email email)
        {
            if (id != email.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(email);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmailExists(email.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(email);
        }

        // GET: Emails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var email = await _context.Email
                .SingleOrDefaultAsync(m => m.Id == id);
            if (email == null)
            {
                return NotFound();
            }

            return View(email);
        }

        // POST: Emails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var email = await _context.Email.SingleOrDefaultAsync(m => m.Id == id);
            _context.Email.Remove(email);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmailExists(int id)
        {
            return _context.Email.Any(e => e.Id == id);
        }
    }
}
