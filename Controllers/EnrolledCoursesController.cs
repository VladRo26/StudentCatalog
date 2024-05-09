using Microsoft.AspNetCore.Mvc;
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

}
