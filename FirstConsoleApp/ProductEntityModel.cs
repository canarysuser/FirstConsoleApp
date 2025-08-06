using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleApp
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public short UnitsInStock { get; set; }
        public decimal UnitPrice { get; set; }  
        public int CategoryId { get; set; }
        public bool Discontinued { get; set; }
        public override string ToString()
        {
            return $"Id: {ProductId}, Name: {ProductName}" + 
                $"Price: {UnitPrice}, Stock: {UnitsInStock} " + 
                $"Category: {CategoryId}, Discontinued: {Discontinued}";
        }
    }
    public class ProductDbContext: DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=(local);database=northwind;integrated security=sspi;trustservercertificate=true");
            optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Trace);
        }

    }
}
