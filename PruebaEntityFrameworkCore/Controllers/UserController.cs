using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PruebaEntityFrameworkCore.Entidades;
using PruebaEntityFrameworkCore.Models;
using PruebaEntityFrameworkCore.Services;

namespace PruebaEntityFrameworkCore.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public UserController(
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._context = context;
        }

        [AllowAnonymous]
        public IActionResult Registry()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registry(RegistryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser() 
            {
                Email = model.Email,
                UserName = model.Email, 
                HelpPassword = model.HelpPassword
            };

            var result = await _userManager.CreateAsync(user, password: model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: true);
                return RedirectToAction("Index", "Home");
            }
            else 
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }
        }

        [AllowAnonymous]
        public IActionResult Login(string message = null)
        {
            if (message is not null) {
                ViewData["message"] = message;
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.Remember, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Nombre de usuario o password incorrecto");
                return View(model);

            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> List(string confirmed = null, string remove = null)
        {
            var userList =  await this._context.Users.ToListAsync();
            var userRoleList = await this._context.UserRoles.ToListAsync();

            var userDtoList = userList.GroupJoin(userRoleList, u => u.Id, ur => ur.UserId, 
                (u, ur) => new UserViewModel  {
                    User = u.UserName,
                    Email = u.Email,
                    Confirmed = u.EmailConfirmed,
                    IsAdmin = ur.Any(ur => ur.UserId == u.Id)
                })
                .OrderBy(u => u.User)
                .ToList();


            var modelo = new UserListViewModel();
            modelo.UserList = userDtoList;
            modelo.MessageConfirmed = confirmed;
            modelo.MessageRemoved = remove;

            return View(modelo);
        }


        [HttpPost]
        // [Authorize(Roles = Constantes.RolAdmin)]
        public  async Task<IActionResult> HacerAdmin(string email)
        {
            var usuario = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario is null)
            {
                return NotFound();
            }

            await _userManager.AddToRoleAsync(usuario, MyConstants.RolAdmin);

            return RedirectToAction("List",
                routeValues: new { confirmed = "Rol asignado correctamente a " + email, remove = ""  });
        }

        [HttpPost]
        // [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> RemoverAdmin(string email)
        {
            var usuario = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario is null)
            {
                return NotFound();
            }

            await _userManager.RemoveFromRoleAsync(usuario, MyConstants.RolAdmin);

            return RedirectToAction("List",
                routeValues: new { confirmed = "", remove = "Rol removido correctamente a " + email });
        }
    
        public IActionResult ChangePassword(string confirmed = null)
        {
            var model = new ChangePasswordViewModel();
            model.OldPassword = " ";
            model.NewPassword = " ";
            model.ConfirmPassword = " ";
            model.MessageConfirmed = confirmed;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View(model);

            }

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction("ChangePassword",
                routeValues: new { confirmed = "El password se ha cambiado con éxito" });

        }
    }
}