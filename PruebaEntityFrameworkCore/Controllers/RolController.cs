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


        public IActionResult RolEdit(string Id)
        {
            // SENTENCIA EN LINQ
            var entity = this._context.Roles
                .Where(p => p.Id == Id).FirstOrDefault();
            
            //VALIDACION SI NO LO ENCUENTRA 
            if (entity == null)
            {
                return RedirectToAction("RolList","Rol");
            }

            //Se asigna la info de la BD al MODELO.
            var model = new RolViewModel();
            model.Id = new Guid(entity.Id);
            model.Name = entity.Name;

            //PASAMOS LA INFORMACION AL MODELO
            return View(model);
        }

        [HttpPost]
        public IActionResult RolEdit(RolViewModel model)
        {
            //Carga la información de la BD
            var entity = this._context.Roles
             .Where(p => p.Id == model.Id.ToString()).First();

            //VALIDACION
            if (entity == null)
            {
                return View(model);
            }
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //REmplaza lo del modelo en el objeto de la BD
            entity.Name = model.Name;

            //Actualiza y guarda
            this._context.Roles.Update(entity);
            this._context.SaveChanges();
            
            //Muestra otravez la lista 
            return RedirectToAction("RolList","Rol");
        }

        public IActionResult RolDeleted(Guid Id)
        {
            var entity = this._context.Roles
            .Where(p => p.Id == Id.ToString()).FirstOrDefault();
            
            if (entity == null)
            {
                return RedirectToAction("RolList","Rol");
            }


            var model = new RolViewModel();
            model.Id = new Guid(entity.Id);
            model.Name = entity.Name;

            return View(model);
        }

        [HttpPost]
        public IActionResult RolDeleted(RolViewModel model)
        {
            bool exits = this._context.Roles.Any(p => p.Id == model.Id.ToString());
            if (!exits)
            {
                return View(model);
            }


            var entity = this._context.Roles
            .Where(p => p.Id == model.Id.ToString()).First();

            this._context.Roles.Remove(entity);
            this._context.SaveChanges();
            
            return RedirectToAction("RolList","Rol");
        }
    }
}