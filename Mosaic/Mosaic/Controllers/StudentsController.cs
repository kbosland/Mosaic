using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mosaic.Models;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Mosaic.Services;
using System.Collections.Generic;
using Mosaic.Repository;

namespace Mosaic.Controllers
{
    public class StudentsController : Controller
    {
        private readonly Models.MosaicContext _context;
        private readonly IStudentAuthentication _service;
        private readonly IEmailAuthentication _emailService;

        public StudentsController(Models.MosaicContext context, IStudentAuthentication service, IEmailAuthentication emailService)
        {
            _context = context;
            _service = service;
            _emailService = emailService;
        }

        //GET: Students/EmailMenu
        public IActionResult EmailMenu()
        {
            return View();
        }

        //GET: Students/CannotEnroll
        public IActionResult CannotEnroll()
        {
            return View();
        }

        //GET: Students/EnrollInClass
        public IActionResult EnrollInClass()
        {
            var student = _context.Student.SingleOrDefault(m => m.Username == HttpContext.Session.GetString("username"));

            _context.Student.Where(x => x.ClassOne == "abc");

            foreach (Student s in _context.Student)
            {
                if (s.ClassOne == "abc")
                {

                }
            }

            var items1 = _context.Class.ToList();

            for (int i = 0; i < items1.Count; i++)
            {
                if (items1[i].NumEnrolled + 1 > items1[i].MaxEnroll)
                {
                    items1.Remove(items1[i]);
                }
            }

            ViewData["Classes"] = items1;
            ViewData["ClassOne"] = student.ClassOne;
            ViewData["ClassTwo"] = student.ClassTwo;
            return View();
        }

        //GET: Students/CannotDrop
        public IActionResult CannotDrop()
        {
            return View();
        }

        //GET: Students/DropClass
        public IActionResult DropClass()
        {
            var student = _context.Student.SingleOrDefault(m => m.Username == HttpContext.Session.GetString("username"));

            ViewData["ClassOne"] = student.ClassOne;
            ViewData["ClassTwo"] = student.ClassTwo;
            ViewData["Classes"] = _context.Class.ToList();
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
            var student = _service.VerifyChangePassword(username, oldPass, newPass);
            ViewData["Username"] = HttpContext.Session.GetString("username");
            ViewData["Usernames"] = _service.ReturnAllUsernames();

            if (student != null)
            {
                _context.Student.Update(student);
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

        // POST: Students/DropClass
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DropClass(string classCode, int result)
        {
            var student = await _context.Student.SingleOrDefaultAsync(m => m.Username == HttpContext.Session.GetString("username"));
            var chosenClass = await _context.Class.SingleOrDefaultAsync(m => m.ClassCode == classCode.ToUpper());
            ViewData["ClassOne"] = student.ClassOne;
            ViewData["ClassTwo"] = student.ClassTwo;
            ViewData["Classes"] = _context.Class.ToList();

            int choice = Convert.ToInt32(result);

            if (choice == 1)
            {
                student.ClassOne = null;
                chosenClass.NumEnrolled--;
                _context.Student.Update(student);
                _context.Class.Update(chosenClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DropClass));
            }
            else if (choice == 2)
            {
                student.ClassTwo = null;
                chosenClass.NumEnrolled--;
                _context.Student.Update(student);
                _context.Class.Update(chosenClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DropClass));
            }
            return RedirectToAction(nameof(CannotDrop));
        }

        // POST: Students/EnrollInClass
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnrollInClass(string classCode, string result)
        {
            var student = await _context.Student.SingleOrDefaultAsync(m => m.Username == HttpContext.Session.GetString("username"));
            var chosenClass = await _context.Class.SingleOrDefaultAsync(m => m.ClassCode == classCode.ToUpper());
            var items1 = _context.Class.ToList();

            for (int i = 0; i < items1.Count; i++)
            {
                if (items1[i].NumEnrolled + 1 > items1[i].MaxEnroll || items1[1].ClassCode.Equals(student.ClassOne) || items1[i].ClassCode.Equals(student.ClassTwo))
                {
                    items1.Remove(items1[i]);
                }
            }

            ViewData["Classes"] = items1;
            ViewData["ClassOne"] = student.ClassOne;
            ViewData["ClassTwo"] = student.ClassTwo;

            int choice = Convert.ToInt32(result);

            if (choice == 1)
            {
                student.ClassOne = classCode.ToUpper();
                chosenClass.NumEnrolled++;
                _context.Student.Update(student);
                _context.Class.Update(chosenClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EnrollInClass));
            }
            else if (choice == 2)
            {
                student.ClassTwo = classCode.ToUpper();
                chosenClass.NumEnrolled++;
                _context.Student.Update(student);
                _context.Class.Update(chosenClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EnrollInClass));
            }

            return RedirectToAction(nameof(CannotEnroll));
        }

        //GET: Home
        public IActionResult Home()
        {
            if (HttpContext.Session.GetString("username") == null || HttpContext.Session.GetString("username").Equals(""))
            {
                return View();
            }
            else if (HttpContext.Session.GetInt32("type") == 0)
            {
                return RedirectToAction("Edit");
            }
            else if (HttpContext.Session.GetInt32("type") == 1)
            {
                return RedirectToAction("Edit", "Professors");
            }

            return View();
        }

        //GET: Students/UsernameTaken
        public IActionResult UsernameTaken()
        {
            return View();
        }

        //GET: Students/Menu
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

        //GET: Students/NotFound
        public IActionResult StudentNotFound()
        {
            return View();
        }

        // GET: Students
        public async Task<IActionResult> Index()
        { 
            var loginSystemContext = _context.Student.Include(s => s.ClassOneNavigation).Include(s => s.ClassTwoNavigation);
            return View(await loginSystemContext.ToListAsync());
        }

        // GET: Students/CreateStudent
        public IActionResult CreateStudent()
        {
            HttpContext.Session.SetString("username", "");
            var items1 = _context.Class.ToList();
            var items2 = _context.Class.ToList();
            for (int i = 0; i < items1.Count; i++)
            {
                if (items1[i].NumEnrolled + 1 > items1[i].MaxEnroll)
                {
                    items1.Remove(items1[i]);
                    items2.Remove(items2[i]);
                }
            }

            items1.Insert(0, new Class { ClassCode = "" });
            items2.Insert(0, new Class { ClassCode = "" });

            ViewData["ClassOne"] = new SelectList(items1, "ClassCode", "ClassCode", string.Empty);
            ViewData["ClassTwo"] = new SelectList(items2, "ClassCode", "ClassCode", string.Empty);
            ViewData["Usernames"] = _service.ReturnAllUsernames();
            return View();
        }

        // POST: Students/CreateStudent
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStudent([Bind("Username,Password,FirstName,LastName,ClassOne,ClassTwo")] Student student)
        {
            HttpContext.Session.SetString("username", "");
            var items1 = _context.Class.ToList();
            var items2 = _context.Class.ToList();
            for (int i = 0; i < items1.Count; i++)
            {
                if (items1[i].NumEnrolled + 1 > items1[i].MaxEnroll)
                {
                    items1.Remove(items1[i]);
                    items2.Remove(items2[i]);
                }
            }
            items1.Insert(0, new Class { ClassCode = "" });
            items2.Insert(0, new Class { ClassCode = "" });

            ViewData["ClassOne"] = new SelectList(items1, "ClassCode", "ClassCode", string.Empty);
            ViewData["ClassTwo"] = new SelectList(items2, "ClassCode", "ClassCode", string.Empty);
            ViewData["Usernames"] = _service.ReturnAllUsernames();

            StudentRepository sRepo = new StudentRepository(_context);
            sRepo.AddStudent(student);
            ModelState.Clear();
            return RedirectToAction("LoginStudent");
        }

        public IActionResult CannotCreateStudent()
        {
            return View();
        }

        // GET: Students/LoginStudent
        public IActionResult LoginStudent()
        {
            ViewData["Users"] = _context.Student.ToList();
            HttpContext.Session.SetString("username", "");
            return View();
        }

        // POST: Students/LoginStudent
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginStudent(string username, string password)
        {
            ViewData["Users"] = _context.Student.ToList();
            if (_service.AllowLogin(username, password) != null)
            {
                HttpContext.Session.SetInt32("type", 0);
                HttpContext.Session.SetString("username", username);
                return RedirectToAction(nameof(Menu));
            }
            else
            {
                ViewData["ErrorMsg"] = "Login failed, incorrect password!";
                return View();
            }
        }

        // GET: Students/Edit
        public async Task<IActionResult> Edit()
        {
            String id = HttpContext.Session.GetString("username");

            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.SingleOrDefaultAsync(m => m.Username == id);
            ViewData["Student"] = student;
            if (student == null)
            {
                return RedirectToAction(nameof(StudentNotFound));
            }

            return View(student);
        }

        // POST: Students/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Username,Password,FirstName,LastName,ClassOne,ClassTwo")] Student student)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    ViewData["Student"] = student;
                    if (student.Password != null)
                    {
                        _context.Update(student);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Username))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (SqlException se)
                {
                    return RedirectToAction(nameof(Edit));
                }
                return RedirectToAction(nameof(Edit));
            }
            return View(student);
        }

        // GET: Students/Delete
        public async Task<IActionResult> Delete()
        {
            String id = HttpContext.Session.GetString("username");
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .SingleOrDefaultAsync(m => m.Username == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed()
        {

            string id = HttpContext.Session.GetString("username");
            var student = await _context.Student.SingleOrDefaultAsync(m => m.Username == id);
            if (student.ClassOne != null)
            {
                var classOne = await _context.Class.SingleOrDefaultAsync(m => m.ClassCode == student.ClassOne);
                classOne.NumEnrolled--;
                _context.Class.Update(classOne);
            }

            if (student.ClassTwo != null)
            {
                var classTwo = await _context.Class.SingleOrDefaultAsync(m => m.ClassCode == student.ClassTwo);
                classTwo.NumEnrolled--;
                _context.Class.Update(classTwo);
            }

            List<Announcement> announcements = _context.Announcement.ToList();
            for (int i = 0; i < announcements.Count; i++)
            {
                if (announcements[i].ProfUsername.Equals(id))
                {
                    _context.Announcement.Remove(announcements[i]);
                }
            }

            _context.Student.Remove(student);
            HttpContext.Session.SetString("username", "");
            await _context.SaveChangesAsync();
            return RedirectToAction("Home", "Students");
        }

        private bool StudentExists(string id)
        {
            return _context.Student.Any(e => e.Username == id);
        }
    }
}
