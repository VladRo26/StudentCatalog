using Microsoft.AspNetCore.Mvc;
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
