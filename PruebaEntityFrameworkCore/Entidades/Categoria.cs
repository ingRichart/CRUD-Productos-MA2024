using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaEntityFrameworkCore.Entidades
{
    public class Categoria
    {
        public Categoria()
        {
            Productos = new List<Producto>();
            Nombre = string.Empty;
            Descripcion = string.Empty;
        }

        public Guid Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public List<Producto> Productos { get; set; }
    }
}