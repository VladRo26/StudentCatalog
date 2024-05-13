using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentCatalog.ContextModels;
using StudentCatalog.Models;
using System.Text.RegularExpressions;

namespace StudentCatalog.Controllers
{
    public class StudentController : Controller
    {
        private readonly DataContext _context;

        public StudentController(DataContext context)
        {
            _context = context;
        }

        public IActionResult ModifyStudent(int id)
        {
           

            List<SelectListItem> groups = _context.Grupe.Select(gr => new SelectListItem {Text = gr.GroupNumber.ToString(), Value = gr.Id.ToString()})
                .ToList();

            groups.Insert(0, new SelectListItem { Text = "No Group", Value = "" });
            if (id == null)
            {
                return NotFound();
            }

            var student = _context.Studenti.Where(s => s.Id == id)
             .Include(s => s.User)
             .Include(s => s.Group)
             .FirstOrDefault();

            if (student == null)
            {
                return NotFound();
            }

            ViewBag.Groups = groups;

            return View(student);
        }

        [HttpPost]
        public IActionResult ModifyStudent(StudentModel studentModel)
        {
            if (!ModelState.IsValid)
            {
                // Reload necessary data for the view if validation fails
                ViewBag.Groups = new SelectList(_context.Grupe, "Id", "Name", studentModel.GroupId);
                ViewBag.Groups.Insert(0, new SelectListItem { Text = "No Group", Value = "" });
                return View(studentModel);
            }

            _context.Update(studentModel);
            _context.SaveChanges();

            return RedirectToAction("Index","User"); // or any other view you want to redirect to
        }
    }

}
