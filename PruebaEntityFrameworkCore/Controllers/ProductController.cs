using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaEntityFrameworkCore.Entidades;
using PruebaEntityFrameworkCore.Models;

namespace PruebaEntityFrameworkCore.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ApplicationDbContext context, ILogger<ProductController> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<IActionResult> ProductList()
        {
            List<ProductModel> products 
            = await _context.Productos
            .Include(p => p.Categoria)
            .Include(s => s.Proveedor)
            .Select(product => new ProductModel()
            {
                Id = product.Id,
                Name = product.Nombre,
                Quantity = product.Cantidad,
                Price = product.Precio,
                Description = product.Descripcion,
                CategoriaName = product.Categoria.Nombre,
                SupplierName = product.Proveedor.Nombre
            }).ToListAsync();

            return View(products);
        }   

        public async Task<IActionResult> ProductAdd()
        {
            ProductModel product = new ProductModel();
            
            product.ListaCategorias = 
                await _context.Categorias.Select(c => new SelectListItem()
                { Value = c.Id.ToString(), Text = c.Nombre }
                ).ToListAsync();

            product.ListSuppliers = 
                _context.Proveedores.Select(p => new SelectListItem()
                { Value = p.Id.ToString(), Text = p.Nombre }
                ).ToList();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> ProductAdd(ProductModel productModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("El modelo del producto no es valido");
                return View(productModel);
            }

            var productEntity = new Producto();
            productEntity.Id = new Guid();
            productEntity.Nombre = productModel.Name;
            productEntity.Cantidad = productModel.Quantity;
            productEntity.Descripcion = productModel.Description;
            productEntity.Precio = productModel.Price;
            productEntity.CategoriaId = productModel.CategoriaId;
            productEntity.ProveedorId = productModel.SupplierId;

            this._context.Productos.Add(productEntity);
            await this._context.SaveChangesAsync();
            
            return RedirectToAction("ProductList","Product");
        }

        public async Task<IActionResult> ProductEdit(Guid Id)
        {
            Producto? product = await this._context.Productos
            .Where(p => p.Id == Id).FirstOrDefaultAsync();
            
            if (product == null)
            {
                return RedirectToAction("ProductList","Product");
            }

            ProductModel model = new ProductModel();
            model.Id = product.Id;
            model.Name = product.Nombre;
            model.Quantity = product.Cantidad;
            model.Description = product.Descripcion;
            model.Price = product.Precio;
            model.CategoriaId  = product.CategoriaId;
            model.SupplierId = product.ProveedorId;
            
            model.ListaCategorias = 
                await _context.Categorias.Select(c => new SelectListItem()
                { Value = c.Id.ToString(), Text = c.Nombre }
                ).ToListAsync();
            
            model.ListSuppliers = 
                await _context.Proveedores.Select(s => new SelectListItem()
                { Value = s.Id.ToString(), Text = s.Nombre }
                ).ToListAsync();
            

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductModel productModel)
        {
            bool exits = await this._context.Productos.AnyAsync(p => p.Id == productModel.Id);
            if (!exits)
            {
                return View(productModel);
            }
            
            if (!ModelState.IsValid)
            {
                return View(productModel);
            }

            Producto productEntity = await this._context.Productos
            .Where(p => p.Id == productModel.Id).FirstAsync();
            productEntity.Nombre = productModel.Name;
            productEntity.Cantidad = productModel.Quantity;
            productEntity.Descripcion = productModel.Description;
            productEntity.Precio = productModel.Price;
            productEntity.CategoriaId = productModel.CategoriaId;
            productEntity.ProveedorId = productModel.SupplierId;

            this._context.Productos.Update(productEntity);
            await this._context.SaveChangesAsync();
            
            return RedirectToAction("ProductList","Product");
        }

        public async Task<IActionResult> ProductDeleted(Guid Id)
        {
            Producto? product = await this._context.Productos
            .Where(p => p.Id == Id).FirstOrDefaultAsync();
            
            if (product == null)
            {
                return RedirectToAction("ProductList","Product");
            }

            ProductModel model = new ProductModel();
            model.Id = product.Id;
            model.Name = product.Nombre;
            model.Quantity = product.Cantidad;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProductDeleted(ProductModel product)
        {
            bool exits = await this._context.Productos.AnyAsync(p => p.Id == product.Id);
            if (!exits)
            {
                return View(product);
            }

            Producto productEntity = await this._context.Productos
            .Where(p => p.Id == product.Id).FirstAsync();
            productEntity.Nombre = product.Name;
            productEntity.Cantidad = product.Quantity;
            productEntity.Descripcion = product.Description;
            productEntity.Precio = product.Price;
            productEntity.CategoriaId = product.CategoriaId;

            this._context.Productos.Remove(productEntity);
            await this._context.SaveChangesAsync();
            
            return RedirectToAction("ProductList","Product");
        }

    }
}