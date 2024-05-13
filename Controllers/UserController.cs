using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentCatalog.ContextModels;
using StudentCatalog.Logic;
using StudentCatalog.Models;
using System;
using System.Linq;

namespace StudentCatalog.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        private readonly DataContext _context;

        public List<UserModel> Users { get; set; }

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Users = _context.Useri.ToList();
            var userStudentMapping = _context.Studenti
                                     .Where(student => student.UserId.HasValue)
                                     .ToDictionary(student => student.UserId.Value, student => student.Id);

            ViewBag.UserStudentMapping = userStudentMapping;

            if (Users == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View("Index", Users);
        }

        [HttpPost("UpdateUserRole")]
        public IActionResult UpdateUserRole([FromBody] UserRoleUpdateModel data)
        {
            var user = _context.Useri.FirstOrDefault(u => u.Id == data.UserId);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            try
            {
                user.Role = Enum.Parse<UserType>(data.Role, true);

                if (user.Role == UserType.Student && !_context.Studenti.Any(s => s.UserId == user.Id))
                {
                    // Create a new Student record if none exists
                    var newStudent = new StudentModel
                    {
                        UserId = user.Id,
                        User = user,
                        YearOfStudy = null, 
                        GroupId = null, 
                        Group = null,
                        IsEnrolled = false
                    };
                    _context.Studenti.Add(newStudent); // Add the new student to the context
                }

                _context.SaveChanges();
                return Json(new { success = true, message = "Role updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }

    public class UserRoleUpdateModel
    {
        public int UserId { get; set; }
        public string Role { get; set; }
    }
}