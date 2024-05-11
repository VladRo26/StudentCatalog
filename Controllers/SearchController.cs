using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentCatalog.ContextModels;
using StudentCatalog.Models;

namespace StudentCatalog.Controllers
{
    public class SearchController : Controller
    {
        private readonly DataContext _context;

        public SearchController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult SearchPortal(string courseName, string teacherName)
        {
            List<CourseModel> courses;
            if (string.IsNullOrEmpty(courseName) && string.IsNullOrEmpty(teacherName))
            {
                courses = _context.Cursuri.Include(course => course.Teacher).ToList();
            }
            else if (string.IsNullOrEmpty(teacherName))
            {
                courses = _context.Cursuri.Include(course => course.Teacher)
                          .Where(course => course.Name.ToLower().Contains(courseName.ToLower()))
                          .ToList();
            }
            else
            {
                courses = _context.Cursuri.Include(course => course.Teacher)
                         .Where(course => course.Teacher.LastName.ToLower().Contains(teacherName.ToLower()))
                         .ToList();

            }
            ViewBag.CourseName = courseName;
            ViewBag.TeacherName = teacherName;
            return View("~/Views/Course/Index.cshtml", courses); // Return the same view with either all courses or filtered courses
        }

    }
}
