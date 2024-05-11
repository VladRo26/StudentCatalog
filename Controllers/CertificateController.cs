using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using StudentCatalog.ContextModels;
using StudentCatalog.Models;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

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

                if(ModelState.IsValid)
                {
                   if(certificate.Student.IsEnrolled) {
                        
                        _context.Adeverinte.Add(certificate);
                        _context.SaveChanges();
                        
                        
                        using (MemoryStream stream = new MemoryStream())
                        {
                            StudentCertificateModel model = certificate;
                            // Dimensiune pagină A4 cu margini ajustate
                            iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);
                            PdfWriter writer = PdfWriter.GetInstance(document, stream);
                            document.Open();

                            // Fonturi pentru titluri și paragrafe
                            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                            Font paragraphFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

                            // Adăugăm un titlu centralizat
                            Paragraph title = new Paragraph("Adeverinta de student", titleFont)
                            {
                                Alignment = Element.ALIGN_CENTER
                            };
                            document.Add(title);

                            // Linie de separare
                            document.Add(new Paragraph("\n"));

                            document.Add(new Paragraph($"Elevul(a)  {model.FirstName} {model.LastName} este inscris la Facultatea de Matematica si Informatica, Specializarea: Informatica Anul de studiu: {model.Student.YearOfStudy} , Grupa: {model.Student.Group.GroupNumber} ", paragraphFont));
                            document.Add(new Paragraph($"Eliberam prezenta pentru a-i servi la : {model.Reason}", paragraphFont));


                            document.Add(new Paragraph($"Numar de telefon: {model.PhoneNumber}", paragraphFont));
                            // Dată eliberare și spațiu pentru semnături
                            document.Add(new Paragraph($"Data eliberării: {model.CreatedDate}", paragraphFont));
                            document.Add(new Paragraph("\n\n\n\n"));

                            // Semnătură Decan/Director și spațiu pentru ștampilă
                            Paragraph signatureSection = new Paragraph("Semnătura decanului/directorului: _____________________", paragraphFont);
                            signatureSection.SpacingBefore = 20;
                            signatureSection.SpacingAfter = 20;
                            document.Add(signatureSection);
                            document.Add(new Paragraph("Stampila instituției", paragraphFont));

                            // Închide documentul PDF
                            document.Close();
                            writer.Close();
                            
                            byte[] bytes = stream.ToArray();
                            return File(bytes, "application/pdf", $"{model.FirstName}_{model.LastName}_Adeverinta.pdf");
                          
                      
                        }
                        

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "You are not enrolled for this University Year");
                    }
                    
                    return View();
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
