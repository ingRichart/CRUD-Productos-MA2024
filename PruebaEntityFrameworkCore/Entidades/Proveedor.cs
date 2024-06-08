using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaEntityFrameworkCore.Entidades
{
    public class Proveedor
    {
        public Guid Id { get; set; }

        public string Nombre { get; set; }

        public List<Producto> Productos { get; set; }
    }
}