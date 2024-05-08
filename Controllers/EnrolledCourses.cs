using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentCatalog.ContextModels;
using StudentCatalog.Models;
using System.Security.Claims;

namespace StudentCatalog.Controllers;

public class EnrolledCourses : Controller
{

    private readonly DataContext _context;
    public List<StudentCoursesModel> CursuriStudent { get; set; }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public EnrolledCourses(DataContext context)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        _context = context;
    }


    public IActionResult StudentCourses()
    {
         CursuriStudent= _context.CursuriStudenti.Include(cursuri =>cursuri.Course )
            //.Where(courseStudent=>courseStudent.StudentId==1)
            .ToList();

            
        return View(CursuriStudent);
    }

}
