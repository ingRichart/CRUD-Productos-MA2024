using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaEntityFrameworkCore.Models
{
    public class RolViewModel
    {
        public Guid Id { get; set;}
        [Required(ErrorMessage = "El {0} es obligatorio")]
        [Display(Name = "Rol")]
        public string Name { get; set;}
    }
}