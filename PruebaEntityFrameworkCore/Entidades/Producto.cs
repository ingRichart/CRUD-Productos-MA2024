using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaEntityFrameworkCore.Entidades
{
    public class Producto
    {
        public Guid Id { get; set; }

        public string Nombre { get; set; }

        public int Cantidad { get; set; }

        public string Descripcion { get; set;}

        public float Precio { get; set; }

        public Guid? CategoriaId { get; set; }

        public Categoria? Categoria { get; set; }

        public Guid? ProveedorId { get; set;}

        public Proveedor? Proveedor { get; set;}
    }
}