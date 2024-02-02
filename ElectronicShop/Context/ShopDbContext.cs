using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ElectronicShop.Models;

namespace ElectronicShop.Context
{
    public class ShopDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server='DESKTOP-C3SM1NS\\SQLEXPRESS';Database='ElectronicShop';Trusted_Connection=True;TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Category> Category { get; set; } = null!;
        public DbSet<Product> Product { get; set; } = null!;
    }
}
