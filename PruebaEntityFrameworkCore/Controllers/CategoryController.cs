using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PruebaEntityFrameworkCore.Entidades;
using PruebaEntityFrameworkCore.Models;

namespace PruebaEntityFrameworkCore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            this._context = context;
        }

        public IActionResult CategoryList()
        {
            List<CategoryProductModel> categories 
            = _context.Categorias.Select(category => new CategoryProductModel()
            {
                Id = category.Id,
                Name = category.Nombre,
                Description = category.Descripcion
            }).ToList();

            return View(categories);
        }   

        public IActionResult CategoryAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CategoryAdd(CategoryProductModel category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var categoryEntity = new Categoria();
            categoryEntity.Id = new Guid();
            categoryEntity.Nombre = category.Name;
            categoryEntity.Descripcion = category.Description;

            this._context.Categorias.Add(categoryEntity);
            this._context.SaveChanges();
            
            return RedirectToAction("CategoryList","Category");
        }

        public IActionResult CategoryEdit(Guid Id)
        {
            // SENTENCIA EN LINQ
            Categoria? category = this._context.Categorias
                .Where(p => p.Id == Id).FirstOrDefault();
            
            //VALIDACION SI NO LO ENCUENTRA 
            if (category == null)
            {
                return RedirectToAction("CategoryList","Category");
            }

            //Se asigna la info de la BD al MODELO.
            CategoryProductModel model = new CategoryProductModel();
            model.Id = category.Id;
            model.Name = category.Nombre;
            model.Description = category.Descripcion;

            //PASAMOS LA INFORMACION AL MODELO
            return View(model);
        }

        [HttpPost]
        public IActionResult CategoryEdit(CategoryProductModel category)
        {
            //Carga la información de la BD
            Categoria categoryEntity = this._context.Categorias
             .Where(p => p.Id == category.Id).First();

            //VALIDACION
            if (categoryEntity == null)
            {
                return View(category);
            }
            
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            //REmplaza lo del modelo en el objeto de la BD
            categoryEntity.Nombre = category.Name;
            categoryEntity.Descripcion = category.Description;

            //Actualiza y guarda
            this._context.Categorias.Update(categoryEntity);
            this._context.SaveChanges();
            
            //Muestra otravez la lista 
            return RedirectToAction("CategoryList","Category");
        }

        public IActionResult CategoryDeleted(Guid Id)
        {
            Categoria? category = this._context.Categorias
            .Where(p => p.Id == Id).FirstOrDefault();
            
            if (category == null)
            {
                return RedirectToAction("CategoryList","Category");
            }


            CategoryProductModel model = new CategoryProductModel();
            model.Id = category.Id;
            model.Name = category.Nombre;
            model.Description = category.Descripcion;

            return View(model);
        }

        [HttpPost]
        public IActionResult CategoryDeleted(CategoryProductModel category)
        {
            bool exits = this._context.Categorias.Any(p => p.Id == category.Id);
            if (!exits)
            {
                return View(category);
            }


            Categoria categoryEntity = this._context.Categorias
            .Where(p => p.Id == category.Id).First();

            this._context.Categorias.Remove(categoryEntity);
            this._context.SaveChanges();
            
            return RedirectToAction("CategoryList","Category");
        }

    }
}