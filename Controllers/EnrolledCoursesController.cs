using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentCatalog.ContextModels;
using StudentCatalog.Models;
using System.Security.Claims;
using StudentCatalog.Logic;

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

        CursuriStudent = _context.CursuriStudenti.Include(cursuri => cursuri.Course)
            .Where(courseStudent => courseStudent.StudentId == Int16.Parse(studentId))
            .ToList();



        ViewData["StudentYear"] = _context.Studenti
                                    .Where(s => s.UserId == Int16.Parse(userId))
                                    .Select(u => u.YearOfStudy)
                                    .FirstOrDefault();

        return View("StudentCourses", CursuriStudent);
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
        newEnrollment.Course = _context.Cursuri.Where(c => c.Id == newEnrollment.CourseId).Include(s=>s.Teacher).FirstOrDefault();
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
        AlertModel alert = new AlertModel(newEnrollment.Student, String.Format("Ai fost Adaugat la cursul {0} predat de {1}", newEnrollment.Course.Name, newEnrollment.Course.Teacher.FirstName+" "+newEnrollment.Course.Teacher.LastName));
        _context.Alerte.Add(alert);
        _context.SaveChanges();

        // Redirect to wherever is appropriate, such as the list of enrolled courses
        return RedirectToAction("Index", "Course");
    }

    [HttpGet]
    public IActionResult CatalogSelection()
    {
        // Populate ViewBag with courses and groups
        string userRole = User.Claims.FirstOrDefault(claim => claim.Type == "Role")?.Value! ?? UserType.UtilizatorNelogat.ToString();
        string userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value!;


        if (userRole.Equals(UserType.Profesor.ToString())) {

            ViewBag.Courses = _context.Cursuri
            .Where(c => c.TeacherId == int.Parse(userId))
            .Select(course => new SelectListItem { Text = course.Name, Value = course.Id.ToString() })
            .ToList();


            var courseIds = _context.Cursuri
                           .Where(c => c.TeacherId == int.Parse(userId))
                           .Select(c => c.Id)
                           .ToList();

            // Fetch unique groups of students who are enrolled in these courses
            ViewBag.Groups = _context.CursuriStudenti
                                        .Include(cs => cs.Student)
                                        .ThenInclude(s => s.Group)
                                        .Where(cs => courseIds.Contains(cs.CourseId.GetValueOrDefault()))
                                        .Select(cs => new SelectListItem { Text = cs.Student.Group.GroupNumber.ToString(), Value = cs.Student.Group.Id.ToString() })
                                        .Distinct()
                                        .ToList();


        }
        else if (!userRole.Equals(UserType.UtilizatorNelogat.ToString()))
        {
            ViewBag.Courses = _context.Cursuri
            .Select(course => new SelectListItem { Text = course.Name, Value = course.Id.ToString() })
            .ToList();


            ViewBag.Groups = _context.Grupe
                .Select(group => new SelectListItem { Text = group.GroupNumber.ToString(), Value = group.Id.ToString() })
                .ToList();

        }


        return View();
    }

    [HttpGet]
    public IActionResult CatalogResults(int? courseId, int? groupId)
    {
        List<StudentCoursesModel> results = new List<StudentCoursesModel>();

        results = _context.CursuriStudenti
                .Include(sc => sc.Student)
                .Include(sc => sc.Student.User)
                .Include(sc => sc.Student.Group)
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
    [HttpPost]
    public ActionResult ExportCatalog(List<StudentCoursesModel> model)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            // Create a Document object
            var document = new Document(PageSize.A4, 25, 25, 30, 30);

            // Create a PDF writer that listens to the document
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

            // Open the document
            document.Open();

            // Add a title
            document.Add(new Paragraph("Catalog Results"));
            document.Add(new Paragraph("\n\n\n"));

            // Create a table with four columns
            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 100;

            // Add headers
            table.AddCell("Student Name");
            table.AddCell("Course Name");
            table.AddCell("Group Number");
            table.AddCell("Grade");

            // Add data rows
            foreach (var item in model)
            {
                table.AddCell(item.Student.User.FirstName);
                table.AddCell(item.Course.Name);
                table.AddCell(item.Student.Group != null ? item.Student.Group.GroupNumber.ToString() : "No group");
                table.AddCell(item.Grade.ToString("0.##"));
            }

            // Add table to document
            document.Add(table);

            // Close the document
            document.Close();

            // Write the PDF to memory stream
            byte[] bytes = memoryStream.ToArray();

            // Send the PDF as a file result
            return File(bytes, "application/pdf", "CatalogResults.pdf");
        }
    }

    [HttpPost]
    public IActionResult UpdateGrade(int modelId,List<StudentCoursesModel> model)
    {
        StudentCoursesModel actualCS = model.Where(cs => cs.Id == modelId).FirstOrDefault();
        StudentCoursesModel newCS= _context.CursuriStudenti.Where(cs => cs.Id == actualCS.Id).Include(cs=>cs.Course).Include(cs=>cs.Student).FirstOrDefault();
        if (newCS!=null )
        {
            if(actualCS.Grade>=0 && actualCS.Grade <= 10)
            {
                if(actualCS.Grade!= _context.CursuriStudenti.Where(cs => cs.Id == modelId).FirstOrDefault().Grade)
                {
                    newCS.Grade = actualCS.Grade;
                    AlertModel alert = new AlertModel(newCS.Student, String.Format("Nota ta de la materia {0} a fost modificata . Nota ta este acum {1}", newCS.Course.Name, newCS.Grade));
                    _context.Alerte.Add(alert);
                    _context.SaveChanges();
                    TempData["UpdateGradeError"] = "Error on update grade!";

                }
               
                return RedirectToAction("CatalogResults");
            }
            TempData["UpdateGradeError"] = "Grade should be between 0 and 10";

        }
        else
        {
            TempData["UpdateGradeError"] = "Error on update grade!";
        }
        return RedirectToAction("CatalogResults");
    }
  

}
