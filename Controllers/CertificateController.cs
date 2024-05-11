using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using StudentCatalog.ContextModels;
using StudentCatalog.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace StudentCatalog.Controllers
{
    public class CertificateController : Controller
    {
        public readonly DataContext _context;
        string userId;
        string studentId;
        StudentModel student;
        public CertificateController(DataContext context) {
            this._context = context;
         
        }

       
        public IActionResult GenerateCertificate()
        {
            userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            studentId = _context.Studenti
                                    .Where(s => s.UserId == int.Parse(userId))
                                    .Select(u => u.Id)
                                    .FirstOrDefault().ToString();
            student = new StudentModel();
            student = _context.Studenti.Where(s => s.Id == int.Parse(studentId)).FirstOrDefault();
            student.Group = _context.Grupe.Where(g => g.Id == student.GroupId).FirstOrDefault();
            ViewData["StudentYear"] = student.YearOfStudy.ToString();
            ViewData["StudentGroup"] = student.Group.GroupNumber.ToString();
            return View();
        }
        [HttpPost]
        public IActionResult GenerateCertificate(StudentCertificateModel certificate)
        {
            if (certificate != null)
            {
                userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    studentId = _context.Studenti
                                            .Where(s => s.UserId == int.Parse(userId))
                                            .Select(u => u.Id)
                                            .FirstOrDefault().ToString();

                    // Load the student with the related User and Group data
                    student= _context.Studenti
                                      .Include(s => s.User)  // Include the User
                                      .Include(s => s.Group) // Include the Group
                                      .FirstOrDefault(s => s.Id == int.Parse(studentId));

                    if (student != null)
                    {
                        ViewData["StudentYear"] = student.YearOfStudy.ToString();
                        ViewData["StudentGroup"] = student.Group?.GroupNumber.ToString();  // Use safe navigation in case Group is null

                        // Assign the loaded student directly to the certificate
                        certificate.Student = student;
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.ValidationState == ModelValidationState.Invalid)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage);

                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                    Console.Write("\n");
                    Console.WriteLine("cac");
                }
            }

            return View(certificate);

        }


    }
}
