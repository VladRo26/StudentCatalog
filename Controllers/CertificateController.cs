using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentCatalog.ContextModels;
using StudentCatalog.Models;

namespace StudentCatalog.Controllers
{
    public class CertificateController : Controller
    {
        public readonly DataContext _context; 
        public CertificateController(DataContext context) {
            this._context = context;
        }
        public IActionResult GenerateCertificate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GenerateCertificate(StudentCertificateModel certificate)
        {
            
            return View();
        }


    }
}
