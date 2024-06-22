using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PruebaEntityFrameworkCore.Entidades;

namespace PruebaEntityFrameworkCore
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Proveedor> Proveedores { get; set;}
    }
}