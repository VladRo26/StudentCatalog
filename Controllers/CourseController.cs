using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentCatalog.ContextModels;
using StudentCatalog.Models;


namespace StudentCatalog.Controllers;


public class CourseController : Controller
{
    private readonly DataContext _context;

    public List<CourseModel>? Courses { get; set; }

    public CourseModel? CurrentCourse { get; set; }

    public CourseController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        Courses = _context.Cursuri.Include(course => course.Teacher).ToList();
        if (Courses == null)
        {
            return RedirectToAction("Error", "Home");
        }
        return View("Index",Courses);
    }

    [HttpGet]

    public IActionResult Course(int courseId)
    {
        CurrentCourse = _context.Cursuri.Where(course => course.Id == courseId).Include(course => course.Teacher).FirstOrDefault();

        if(CurrentCourse == null)
        {
            return RedirectToAction("Error", "Home");
        }

        return View(CurrentCourse);
    }

    [HttpGet]
    public IActionResult AddCourse()
    {
        List<SelectListItem> teachers = _context.Useri.Where(teacher => teacher.Role == Logic.UserType.Profesor).
            Select(teacher => new SelectListItem { Text = teacher.LastName, Value = teacher.Id.ToString() })
            .ToList();

        teachers.Insert(0, new SelectListItem { Text = "No Teacher", Value = "" });
        ViewBag.Teachers = teachers;
        return View();
    }

    [HttpPost]
    public IActionResult AddCourse(CourseModel newCourse)
    {
        if (!ModelState.IsValid)
        {
            List<SelectListItem> teachers = _context.Useri.Where(teacher => teacher.Role == Logic.UserType.Profesor).
           Select(teacher => new SelectListItem { Text = teacher.LastName, Value = teacher.Id.ToString() })
           .ToList();

            teachers.Insert(0, new SelectListItem { Text = "No Teacher", Value = "" });

            ViewBag.Teachers = teachers;
            return View(newCourse);

        }

        // If "No Teacher" is selected, set TeacherId to null
        if (newCourse.TeacherId == null)
        {
            newCourse.Teacher = null; // Ensure the Teacher object is null
        }
        else
        {
            // Retrieve the teacher for the selected TeacherId
            newCourse.Teacher = _context.Useri.FirstOrDefault(user => user.Id == newCourse.TeacherId);
        }

        _context.Add(newCourse);
        _context.SaveChanges();
        return RedirectToAction("Index");

    }

    [HttpGet]

    public IActionResult ModifyCourse(int courseId)
    {
        // Retrieve teachers for dropdown list
        List<SelectListItem> teachers = _context.Useri.Where(teacher => teacher.Role == Logic.UserType.Profesor)
            .Select(teacher => new SelectListItem { Text = teacher.LastName, Value = teacher.Id.ToString() })
            .ToList();

        // Include an option for no teacher
        teachers.Insert(0, new SelectListItem { Text = "No Teacher", Value = "" });

        ViewBag.Teachers = teachers;

        // Retrieve course to modify
        CourseModel course = _context.Cursuri
            .Where(course => course.Id == courseId)
            .Include(course => course.Teacher)
            .FirstOrDefault();

        if (course == null)
        {
            return RedirectToPage("Error", "Home");
        }

        return View(course);
    }

    [HttpPost]
    public IActionResult ModifyCourse(CourseModel course)
    {
        if  (!ModelState.IsValid)
        {
            // If model state is not valid, reload the view with validation errors
            List<SelectListItem> teachers = _context.Useri
                .Where(teacher => teacher.Role == Logic.UserType.Profesor)
                .Select(teacher => new SelectListItem { Text = teacher.LastName, Value = teacher.Id.ToString() })
                .ToList();

            // Include an option for no teacher
            teachers.Insert(0, new SelectListItem { Text = "No Teacher", Value = "" });

            ViewBag.Teachers = teachers;
            return View(course);
        }

        // If "No Teacher" is selected, set TeacherId to null
        if (course.TeacherId == null)
        {
            course.TeacherId = null;
        }
        else
        {
            // Otherwise, retrieve the teacher for the selected TeacherId
            course.Teacher = _context.Useri.FirstOrDefault(user => user.Id == course.TeacherId);
        }

        // Update the course and save changes
        _context.Update(course);
        _context.SaveChanges();

        return View("Course", course);
    }


    [HttpGet]
    public IActionResult DeleteCourse(int courseId)
    {
        CourseModel? course = _context.Cursuri
           .Where(course => course.Id == courseId).Include(course => course.Teacher).FirstOrDefault();


        if (course == null)
        {
            return RedirectToAction("Error", "Home");
        }

        List<StudentCoursesModel> studentCourses = _context.CursuriStudenti
        .Where(sc => sc.CourseId == courseId)
        .ToList();

        if (studentCourses.Any())
        {
            _context.CursuriStudenti.RemoveRange(studentCourses);
        }

        _context.Remove(course);
        _context.SaveChanges();
        return RedirectToAction("Index");


    }


}
