using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentCatalog.ContextModels;
using StudentCatalog.Logic;
using StudentCatalog.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace StudentCatalog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;
        public HomeController(ILogger<HomeController> logger,DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            string userRole = User?.Claims.FirstOrDefault(claim => claim.Type == "Role")?.Value;
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0") ;
            if(userRole == UserType.Student.ToString())
            {
                int studentId = _context.Studenti.Where(s => s.UserId == userId).Select(s=>s.Id).FirstOrDefault();
                List<AlertModel> alerts = _context.Alerte.Where(a=>a.StudentId == studentId && a.IsRead==false).ToList();
                return View("StudentIndex",alerts);
            }
            return View();
        }

        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
