using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaEntityFrameworkCore.Models;

namespace PruebaEntityFrameworkCore.Controllers
{
    public class RolController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            this._context = context;
            this._roleManager = roleManager;
        }

        public async Task<IActionResult> RolList()
        {
            var list =  await _context.Roles
            .Select(r => new RolViewModel()
            {
                Id = new Guid(r.Id),
                Name = r.Name
            })
            .ToListAsync();
            
            return View(list);
        }

        public IActionResult RolAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RolAdd(RolViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            IdentityRole entity = new IdentityRole()
            {
                Name = model.Name
            };

            var result = await _roleManager.CreateAsync(entity);

            if (result.Succeeded)
            {
                return RedirectToAction("RolList", "Rol");
            }

            foreach (IdentityError error in result.Errors) 
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);

        }

    }
}