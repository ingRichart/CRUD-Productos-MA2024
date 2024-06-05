using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PruebaEntityFrameworkCore.Models
{
    public class ProductModel
    {
        public ProductModel()
        {
            ListaCategorias = new List<SelectListItem>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public Guid? CategoriaId { get; set; }

        public CategoryProductModel? Categoria { get; set; }

        public string? CategoriaName { get; set; }

        public List<SelectListItem> ListaCategorias { get; set; }
    }
}