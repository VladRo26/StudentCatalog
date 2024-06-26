﻿using Microsoft.AspNetCore.Authentication.Cookies;
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
                        model.Role = Logic.UserType.RolNeasignat;
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
                    UserModel? user = context.Useri.Where(user => user.Username.ToLower() == model.Username.ToLower() && user.Password == model.Password).FirstOrDefault();
                    
                    if (user!=null)
                    {
                    List<Claim> claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Username),
                            new Claim("Role", user.Role.ToString()),
                            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                            new Claim("FirstName", user.FirstName),
                            new Claim("LastName", user.LastName)
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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        if (User.Identity.IsAuthenticated == false)
        {
            return RedirectToAction("Index", "Home");
        }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}
