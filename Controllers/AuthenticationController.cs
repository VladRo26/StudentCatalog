using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using StudentCatalog.ContextModels;
using StudentCatalog.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;

namespace StudentCatalog.Controllers;
public class AuthenticationController : Controller
{

    private readonly DataContext context;

    public AuthenticationController(DataContext context)
    {
        this.context = context;
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]

    public IActionResult Register(UserModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (context.Useri.Where(user => user.Username!.ToLower() == model.Username.ToLower()).Count() > 0)
                    ModelState.AddModelError(string.Empty, "The username is already taken!");

                else
                {
                    try
                    {
                        context.Useri.Add(model);
                        context.SaveChanges();
                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Error creating account: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }
        return View(model);
    }

    [HttpGet]

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserModel model)
    {
       
            try
            {
                if(!model.Username.IsNullOrEmpty() && !model.Password.IsNullOrEmpty())
                {
                    if (context.Useri.Where(user => user.Username.ToLower() == model.Username.ToLower() && user.Password == model.Password).Count() > 0)
                {
                    List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim("Role", model.Role.ToString())
                };
                    var claimIdentity = new ClaimsIdentity(claims, "AuthenticationCookie");
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));
                    return RedirectToAction("Index", "Home");
                }
                    else
                        ModelState.AddModelError(String.Empty, "Invalid username or password!");
                }
                
            
                  
            }catch (Exception ex)
            {               
                ModelState.AddModelError(String.Empty, "Error logging in: " + ex.Message);
            }
        return View(model);
      
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        if (User.Identity.IsAuthenticated == false)
        {
            return RedirectToAction("Index", "Home");
        }
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}
