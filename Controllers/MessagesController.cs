using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentCatalog.ContextModels;
using StudentCatalog.Logic;
using StudentCatalog.Models;
using System.Security.Claims;

namespace StudentCatalog.Controllers
{
    public class MessagesController : Controller
    {
        private readonly DataContext _context;

        public MessagesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult CreateMessage()
        {
            // Get all teachers for the dropdown
            var teachers = _context.Useri
                .Where(u => u.Role == UserType.Profesor)
                .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.FirstName + " " + u.LastName })
                .ToList();

            ViewBag.Teachers = teachers;
            return View();
        }

        [HttpPost]
        public IActionResult LoadMessage(MessagesModel model)
        {
            if (ModelState.IsValid)
            {
                // Set the sender to the currently logged-in user

                var senderId = Convert.ToInt32(User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value!);
                model.SenderId = senderId;
                model.Sender = _context.Useri.Where(u => u.Id == model.SenderId).FirstOrDefault();
                model.TimeStamp = DateTime.UtcNow;

                // Save to the database
                _context.Mesaje.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            // Reload teacher list in case of errors
            ViewBag.TeacherList = _context.Useri
                .Where(u => u.Role == UserType.Profesor)
                .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.FirstName + " " + u.LastName })
                .ToList();

            return View(model);
        }

        public ActionResult Inbox()
        {
            // Get the current teacher's ID
            var teacherId = Convert.ToInt32(User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value!);

            // Retrieve messages for this specific teacher
            var messages = _context.Mesaje
                .Where(m => m.ReceiverId == teacherId)
                .Include(m => m.Sender)
                .OrderByDescending(m => m.TimeStamp)
                .ToList();

            return View(messages);
        }

        public ActionResult Details(int id)
        {
            var message = _context.Mesaje
                .Include(m => m.Sender)
                .FirstOrDefault(m => m.Id == id);

            // Ensure the message exists and is for the current teacher
            if (message == null || (message.ReceiverId != Convert.ToInt32(User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value!)))
            {
                return RedirectToPage("Error","Home");
            }

            return View(message);
        }
    }



}
