using Bliss_Programma.Data;
using Bliss_Programma.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bliss_Programma.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AddAdminController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        ApplicationDbContext db;
             
        public AddAdminController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            db = context;
        }
        public IActionResult Index()
        {
            var users = from u in db.Users
                        select new AllUser
                        {
                            Email = u.Email,
                            Id = u.Id,
                            Role = u.Role,
                            Name = u.Name,
                            Prioriteit = u.Prioriteit
                        };
            return View(users.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email ,Name = model.Name,Role = model.Role,EmailConfirmed = true, Prioriteit = model.Prioriteit};
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    
                    if(model.IsAdmin==true)
                    {
                        await _userManager.AddToRoleAsync(user, "ADMIN");
                    }
                    



                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        public IActionResult Delete(string id)
        {
            var user = db.Users.Where(c => c.Id == id).SingleOrDefault();
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
