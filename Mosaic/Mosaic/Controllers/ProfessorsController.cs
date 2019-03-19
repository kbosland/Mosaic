using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mosaic.Models;
using Mosaic.Services;

namespace Mosaic.Controllers
{
    public class ProfessorsController : Controller
    {
        private readonly Models.MosaicContext _context;
        private readonly IProfAuthentication _service;
        private readonly IEmailAuthentication _emailService;

        public ProfessorsController(Models.MosaicContext context, IProfAuthentication service, IEmailAuthentication emailService)
        {
            _context = context;
            _service = service;
            _emailService = emailService;
        }

        //GET: Professors/EmailMenu
        public IActionResult EmailMenu()
        {
            var prof = _context.Professor.SingleOrDefault(m => m.Username == HttpContext.Session.GetString("username"));
            ViewData["ClassOne"] = prof.ClassOne;
            return View();
        }

        //GET: Students/ChangePassword
        public IActionResult ChangePassword()
        {
            ViewData["Username"] = HttpContext.Session.GetString("username");
            ViewData["Usernames"] = _service.ReturnAllUsernames();
            return View();
        }

        //POST: Students/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string oldPass, string newPass, string username)
        {
            var professor = _service.VerifyChangePassword(username, oldPass, newPass);
            ViewData["Username"] = HttpContext.Session.GetString("username");
            ViewData["Usernames"] = _service.ReturnAllUsernames();

            if (professor != null)
            {
                _context.Professor.Update(professor);
                await _context.SaveChangesAsync();
                ViewData["ErrorMessage"] = "Password change was successful.";
                return View();
            }
            else
            {
                ViewData["ErrorMessage"] = "Incorrect password, change password attempt failed.";
                return View();
            }

        }

        //GET: Professors/TeachAClass
        public IActionResult TeachAClass()
        {
            var professor = _context.Professor.SingleOrDefault(m => m.Username == HttpContext.Session.GetString("username"));
            var classList = _context.Class.ToList();

            for (int i = 0; i < classList.Count; i++)
            {
                if (classList[i].ProfessorId != null)
                {
                    classList.Remove(classList[i]);
                }
            }

            ViewData["ClassOne"] = professor.ClassOne;
            ViewData["Classes"] = classList;

            return View();
        }

        // POST: Professors/TeachAClass
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TeachAClass(string classCode)
        {
            var professor = await _context.Professor.SingleOrDefaultAsync(m => m.Username == HttpContext.Session.GetString("username"));
            var classList = _context.Class.ToList();
            var chosenClass = await _context.Class.SingleOrDefaultAsync(m => m.ClassCode == classCode);

            for (int i = 0; i < classList.Count; i++)
            {
                if (classList[i].ProfessorId != null)
                {
                    classList.Remove(classList[i]);
                }
            }

            ViewData["ClassOne"] = professor.ClassOne;
            ViewData["Classes"] = classList;

            professor.ClassOne = classCode.ToUpper();
            chosenClass.ProfessorId = professor.Username;

            _context.Professor.Update(professor);
            _context.Class.Update(chosenClass);
            await _context.SaveChangesAsync();
            return RedirectToAction("TeachAClass");
        }

        //GET: Professors/DropTeachingAClass
        public IActionResult DropTeachingAClass()
        {
            var professor = _context.Professor.SingleOrDefault(m => m.Username == HttpContext.Session.GetString("username"));
            var classList = _context.Class.ToList();

            ViewData["ClassOne"] = professor.ClassOne;
            ViewData["Classes"] = classList;

            return View();
        }

        //POST: Professors/DropTeachingAClass
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DropTeachingAClass(string classCode)
        {
            var professor = await _context.Professor.SingleOrDefaultAsync(m => m.Username == HttpContext.Session.GetString("username"));
            var classList = _context.Class.ToList();
            var chosenClass = await _context.Class.SingleOrDefaultAsync(m => m.ClassCode == classCode);

            ViewData["ClassOne"] = professor.ClassOne;
            ViewData["Classes"] = classList;

            professor.ClassOne = null;
            chosenClass.ProfessorId = null;

            _context.Professor.Update(professor);
            _context.Class.Update(chosenClass);
            await _context.SaveChangesAsync();
            return RedirectToAction("DropTeachingAClass");
        }

        //GET: Professors/Menu
        public IActionResult Menu()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("username", "");
            HttpContext.Session.SetInt32("type", -1);
            return RedirectToAction("Home");
        }

        // GET: Professors/LoginProf
        public IActionResult LoginProf()
        {
            HttpContext.Session.SetString("username", "");
            ViewData["Users"] = _context.Professor.ToList();
            return View();
        }

        // POST: Professors/LoginProf
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginProf(string username, string password)
        {
            ViewData["Users"] = _context.Professor.ToList();
            if (_service.AllowLogin(username, password) != null)
            {
                HttpContext.Session.SetString("username", username);
                HttpContext.Session.SetInt32("type", 1);
                return RedirectToAction("Menu");
            }
            else
            {
                ViewData["ErrorMsg"] = "Login failed, incorrect password!";
                return View();
            }
        }

        // GET: Professors
        public async Task<IActionResult> Index()
        {
            var loginSystemContext = _context.Professor.Include(p => p.ClassOneNavigation);
            return View(await loginSystemContext.ToListAsync());
        }

        // GET: Professors/CreateProf
        public IActionResult CreateProf()
        {
            HttpContext.Session.SetString("username", "");
            var items1 = _context.Class.ToList();
            for (int i = 0; i < items1.Count; i++)
            {
                if (items1[i].ProfessorId != null)
                {
                    items1.Remove(items1[i]);
                }
            }

            items1.Insert(0, new Class { ClassCode = "" });

            ViewData["ClassOne"] = new SelectList(items1, "ClassCode", "ClassCode", string.Empty);
            ViewData["Usernames"] = _service.ReturnAllUsernames();
            return View();
        }

        // POST: Professors/CreateProf
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProf([Bind("Username,Password,FirstName,LastName,ClassOne")] Professor professor)
        {
            var items1 = _context.Class.ToList();
            for (int i = 0; i < items1.Count; i++)
            {
                if (items1[i].ProfessorId != null)
                {
                    items1.Remove(items1[i]);
                }
            }

            items1.Insert(0, new Class { ClassCode = "" });

            ViewData["ClassOne"] = new SelectList(items1, "ClassCode", "ClassCode", string.Empty);
            ViewData["Usernames"] = _service.ReturnAllUsernames();

            var classOne = await _context.Class.SingleOrDefaultAsync(m => m.ClassCode == professor.ClassOne);
            if (classOne != null) { classOne.ProfessorId = professor.Username; _context.Update(classOne); } else { professor.ClassOne = null; }
            professor.Password = _service.EncryptPassword(professor.Password);
            _context.Add(professor);
            await _context.SaveChangesAsync();
            ModelState.Clear();

            return RedirectToAction("LoginProf");
        }

        // GET: Professors/Edit
        public async Task<IActionResult> Edit()
        {
            String id = HttpContext.Session.GetString("username");
            if (id == null)
            {
                return NotFound();
            }

            var professor = await _context.Professor.SingleOrDefaultAsync(m => m.Username == id);
            ViewData["Professor"] = professor;
            if (professor == null)
            {
                return NotFound();
            }

            return View(professor);
        }

        // POST: Professors/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Username,Password,FirstName,LastName,ClassOne")] Professor professor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ViewData["Professor"] = professor;
                    if (professor.Password != null)
                    {
                        _context.Update(professor);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfessorExists(professor.Username))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            return View(professor);
        }

        // GET: Professors/Delete
        public async Task<IActionResult> Delete()
        {
            string id = HttpContext.Session.GetString("username");

            if (id == null)
            {
                return NotFound();
            }

            var professor = await _context.Professor.SingleOrDefaultAsync(m => m.Username == id);

            if (professor == null)
            {
                return NotFound();
            }

            return View(professor);
        }

        // POST: Professors/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed()
        {
            string id = HttpContext.Session.GetString("username");

            var professor = await _context.Professor.SingleOrDefaultAsync(m => m.Username == id);
            var classOne = await _context.Class.SingleOrDefaultAsync(m => m.ClassCode == professor.ClassOne);
            classOne.ProfessorId = null;
            _context.Professor.Remove(professor);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("username", "");
            return RedirectToAction("Home", "Students");
        }

        private bool ProfessorExists(string id)
        {
            return _context.Professor.Any(e => e.Username == id);
        }
    }
}

