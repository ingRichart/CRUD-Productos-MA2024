using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaEntityFrameworkCore.Entidades;
using PruebaEntityFrameworkCore.Models;

namespace PruebaEntityFrameworkCore.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            this._context = context;
        }

        public IActionResult ProductList()
        {
            List<ProductModel> products 
            = _context.Productos.Select(product => new ProductModel()
            {
                Id = product.Id,
                Name = product.Nombre,
                Quantity = product.Cantidad,
                Description = product.Descripcion
            }).ToList();

            return View(products);
        }   

        public IActionResult ProductAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProductAdd(ProductModel product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            var productEntity = new Producto();
            productEntity.Id = new Guid();
            productEntity.Nombre = product.Name;
            productEntity.Cantidad = product.Quantity;
            productEntity.Descripcion = product.Description;

            this._context.Productos.Add(productEntity);
            this._context.SaveChanges();
            
            return RedirectToAction("ProductList","Product");
        }

        public IActionResult ProductEdit(Guid Id)
        {
            Producto? product = this._context.Productos
            .Where(p => p.Id == Id).FirstOrDefault();
            
            if (product == null)
            {
                return RedirectToAction("ProductList","Product");
            }

            ProductModel model = new ProductModel();
            model.Id = product.Id;
            model.Name = product.Nombre;
            model.Quantity = product.Cantidad;
            model.Description = product.Descripcion;

            return View(model);
        }

        [HttpPost]
        public IActionResult ProductEdit(ProductModel product)
        {
            // bool exits = this._context.Productos.Any(p => p.Id == product.Id);
            // if (!exits)
            // {
            //     return View(product);
            // }
            
            // if (!ModelState.IsValid)
            // {
            //     return View(product);
            // }

            Producto productEntity = this._context.Productos
            .Where(p => p.Id == product.Id).First();
            productEntity.Nombre = product.Name;
            productEntity.Cantidad = product.Quantity;
            productEntity.Descripcion = product.Description;

            this._context.Productos.Update(productEntity);
            this._context.SaveChanges();
            
            return RedirectToAction("ProductList","Product");
        }

        public IActionResult ProductDeleted(Guid Id)
        {
            Producto? product = this._context.Productos
            .Where(p => p.Id == Id).FirstOrDefault();
            
            if (product == null)
            {
                return RedirectToAction("ProductList","Product");
            }

            ProductModel model = new ProductModel();
            model.Id = product.Id;
            model.Name = product.Nombre;
            model.Quantity = product.Cantidad;
            model.Description = product.Descripcion;

            return View(model);
        }

        [HttpPost]
        public IActionResult ProductDeleted(ProductModel product)
        {
            // bool exits = this._context.Productos.Any(p => p.Id == product.Id);
            // if (!exits)
            // {
            //     return View(product);
            // }
            
            // if (!ModelState.IsValid)
            // {
            //     return View(product);
            // }

            Producto productEntity = this._context.Productos
            .Where(p => p.Id == product.Id).First();
            productEntity.Nombre = product.Name;
            productEntity.Cantidad = product.Quantity;
            productEntity.Descripcion = product.Description;

            this._context.Productos.Remove(productEntity);
            this._context.SaveChanges();
            
            return RedirectToAction("ProductList","Product");
        }

    }
}