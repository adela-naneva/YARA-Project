using Microsoft.EntityFrameworkCore;
using WarehouseManagement.API.Models.Domain;

namespace WarehouseManagement.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Inventory> Inventory { get; set; }

    }
}