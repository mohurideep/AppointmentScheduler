using AppointmentScheduler.Models;
using AppointmentScheduler.Models.ViewModels;
using AppointmentScheduler.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduler.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        public AccountController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _dbcontext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Email);
                    HttpContext.Session.SetString("UserName", user.Name);

                    return RedirectToAction("Index", "Appointment");
                }

                ModelState.AddModelError("", "Invalid Login Attempt");
            }
            return View(model);
        }

        public async Task<IActionResult> Register()
        {
        //    if(!_roleManager.RoleExistsAsync(Helper.Admin).GetAwaiter().GetResult())
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole(Helper.Admin));
        //        await _roleManager.CreateAsync(new IdentityRole(Helper.Doctor));
        //        await _roleManager.CreateAsync(new IdentityRole(Helper.Patient));
        //    }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Name=model.Name,
                    UserName=model.Email,
                    Email=model.Email
                };

                var result = await _userManager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.RoleName);
                    if (!User.IsInRole(Helper.Admin))
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    }
                    else
                    {
                        TempData["newAdminSignup"] = user.Name;
                    }
                    return RedirectToAction("Index", "Appointment");
                }
                foreach(var error in result.Errors) { ModelState.AddModelError("", error.Description); }
                
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
