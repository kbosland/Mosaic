using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mosaic.Models;
using Microsoft.AspNetCore.Http;

namespace Mosaic.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly MosaicContext _context;

        public AnnouncementsController (MosaicContext context)
        {
            _context = context;
        }

        //GET: Announcements/MakeAnnouncement
        public IActionResult MakeAnnouncement()
        {
            ViewData["ClassOne"] = _context.Professor.SingleOrDefault(m => m.Username == HttpContext.Session.GetString("username")).ClassOne;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //POST: Announcements/MakeAnnouncement
        public async Task<IActionResult> MakeAnnouncement(string announcementText)
        {

            var prof = await _context.Professor.SingleOrDefaultAsync(m => m.Username == HttpContext.Session.GetString("username"));
            ViewData["ClassOne"] = prof.ClassOne;
            string classCode = prof.ClassOne;
            Announcement announcement = new Announcement { AnnouncementText = announcementText, ClassCode = classCode, ProfUsername = prof.Username };
            _context.Announcement.Add(announcement);
            await _context.SaveChangesAsync();
            return View();
        }

        //GET: Announcements/ViewAnnouncements
        public IActionResult ViewAnnouncements()
        {
            string username = HttpContext.Session.GetString("username");
            var student = _context.Student.SingleOrDefault(m => m.Username == username);
            if (student != null)
            {
                ViewData["ClassOne"] = student.ClassOne + "";
                ViewData["ClassTwo"] = student.ClassTwo + "";

                List<Announcement> classOneAnnouncements = new List<Announcement>();
                List<Announcement> classTwoAnnouncements = new List<Announcement>();
                List<Announcement> announcements = _context.Announcement.ToList();

                for (int i = 0; i < announcements.Count(); i++)
                {
                    if (announcements[i].ClassCode.Equals(student.ClassOne))
                    {
                        classOneAnnouncements.Add(announcements[i]);
                    } else if (announcements[i].ClassCode.Equals(student.ClassTwo))
                    {
                        classTwoAnnouncements.Add(announcements[i]);
                    }
                }

                ViewData["C1"] = classOneAnnouncements;
                ViewData["C2"] = classTwoAnnouncements;
                return View();
            }
            return NotFound();
        }
    }
}
