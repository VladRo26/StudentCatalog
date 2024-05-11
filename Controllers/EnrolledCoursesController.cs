using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentCatalog.ContextModels;
using StudentCatalog.Models;
using System.Security.Claims;

namespace StudentCatalog.Controllers;

public class EnrolledCoursesController : Controller
{

    private readonly DataContext _context;
    public List<StudentCoursesModel>? CursuriStudent { get; set; }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public EnrolledCoursesController(DataContext context)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        _context = context;
    }


    public IActionResult StudentCourses()
    {

        string userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value!;
        string studentId = _context.Studenti
                                    .Where(s => s.UserId == Int16.Parse(userId))
                                    .Select(u => u.Id)
                                    .FirstOrDefault().ToString();

        CursuriStudent = _context.CursuriStudenti.Include(cursuri =>cursuri.Course )
            .Where(courseStudent=>courseStudent.StudentId==Int16.Parse(studentId))
            .ToList();



        ViewData["StudentYear"] = _context.Studenti
                                    .Where(s=>s.UserId==Int16.Parse(userId))
                                    .Select(u=>u.YearOfStudy)
                                    .FirstOrDefault();    

        return View("StudentCourses",CursuriStudent);
    }

    [HttpGet]
    public IActionResult AddStudentToCourse(int courseId)
    {
        CourseModel course = _context.Cursuri.Include(c => c.Teacher).Where(c => c.Id == courseId).FirstOrDefault();
        if (course == null)
        {
            return RedirectToAction("Error", "Home");
        }
        var enrolledStudentIds = _context.CursuriStudenti
        .Where(enrollment => enrollment.CourseId == courseId)
        .Select(enrollment => enrollment.StudentId)
        .ToList();

        List<SelectListItem> students = _context.Studenti.Where(student => student.YearOfStudy == course.YearCourse && !enrolledStudentIds.Contains(student.Id))
            .Select(students => new SelectListItem { Text = students.User.LastName, Value = students.Id.ToString() })
            .ToList();

        if (!students.Any())
        {
            TempData["Message"] = "No students are available to enroll in this course.";
            return RedirectToAction("AddStudentError", "Course");
        }

        ViewBag.Students = students;
        ViewBag.CourseId = courseId;

        return View(); // Ensure this View exists in the correct folder
    }

    [HttpPost]
    public IActionResult AddStudentToCourse(StudentCoursesModel newEnrollment)
    {
        newEnrollment.Course = _context.Cursuri.Where(c => c.Id == newEnrollment.CourseId).FirstOrDefault();
        newEnrollment.Student = _context.Studenti.Where(s => s.Id == newEnrollment.StudentId).FirstOrDefault();

        if (!ModelState.IsValid)
        {
            //var errorKeys = ModelState.Keys;

            //var errors = new List<string>();
            //foreach (var key in errorKeys)
            //{
            //    var errorMessages = ModelState[key].Errors.Select(error => error.ErrorMessage);
            //    Console.WriteLine(errorMessages);
            //}

            List<SelectListItem> students = _context.Studenti.Where(student => student.YearOfStudy == newEnrollment.Course.YearCourse)
                .Select(students => new SelectListItem { Text = students.User.LastName, Value = students.Id.ToString() })
                .ToList();

            ViewBag.Students = students;
            ViewBag.CourseId = newEnrollment.CourseId;

            return View(newEnrollment);

        }
        _context.CursuriStudenti.Add(newEnrollment);
        _context.SaveChanges();

        // Redirect to wherever is appropriate, such as the list of enrolled courses
        return RedirectToAction("Index","Course");
    }

    [HttpGet]
    public IActionResult CatalogSelection()
    {
        // Populate ViewBag with courses and groups
        ViewBag.Courses = _context.Cursuri
            .Select(course => new SelectListItem { Text = course.Name, Value = course.Id.ToString() })
            .ToList();

        ViewBag.Groups = _context.Grupe
            .Select(group => new SelectListItem { Text = group.GroupNumber.ToString(), Value = group.Id.ToString() })
            .ToList();

        return View("CatalogSelection");
    }

    [HttpGet]
    public IActionResult CatalogResults(int? courseId, int? groupId)
    {
        List<StudentCoursesModel> results = new List<StudentCoursesModel>();

        results = _context.CursuriStudenti
                .Include(sc => sc.Student)
                .Include(sc => sc.Student.User)
                .Include(sc =>sc.Student.Group)
                .Include(sc => sc.Course)
                .ToList();

        // Apply the course filter
        if (courseId.HasValue)
        {
            results = _context.CursuriStudenti
                .Where(sc => sc.CourseId == courseId.Value)
                .ToList();
        }

        // Apply the group filter if set
        if (groupId.HasValue)
        {
            // Ensure results are filtered by group ID
            results = results
                .Where(sc => sc.Student.GroupId == groupId.Value)
                .ToList();
        }

        // Retrieve all courses and groups to populate the dropdowns
        var courses = _context.Cursuri.Select(c => new
        {
            c.Id,
            c.Name
        }).ToList();

        var groups = _context.Grupe.Select(g => new
        {
            g.Id,
            g.GroupNumber
        }).ToList();

        // Pass the data to the view through ViewBag
        ViewBag.Courses = courses;
        ViewBag.Groups = groups;

        return View("CatalogResults", results);
    }
}
