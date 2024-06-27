using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            ListSuppliers = new List<SelectListItem>();
        }

        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido ")]
        [Display(Name = "Descripción ")]
        public string Description { get; set; }

        [Display(Name = "Precio")]
        [Range(1, float.MaxValue, ErrorMessage = "El precio debe ser mayor que 0")]  
        public float Price { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido ")]
        public Guid CategoriaId { get; set; }

        public CategoryProductModel? Categoria { get; set; }

        public string? CategoriaName { get; set; }

        public List<SelectListItem> ListaCategorias { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido ")]
        public Guid SupplierId { get; set; }

        public SupplierModel? SupplierModel { get; set; }

        public string? SupplierName { get; set; }    

        public List<SelectListItem> ListSuppliers { get; set;}

    }    
}