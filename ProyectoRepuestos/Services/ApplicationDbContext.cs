using Microsoft.EntityFrameworkCore;
using ProyectoRepuestos.Models;
namespace ProyectoRepuestos.Services;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Repuesto> Repuestos { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Repuesto>().HasData(
            new Repuesto { Id = 1, Name = "Repuesto1", Description = "Descripción del Repuesto1", Price = 100, StockQuantity = 10 },
            new Repuesto { Id = 2, Name = "Repuesto2", Description = "Descripción del Repuesto2", Price = 200, StockQuantity = 20 },
            new Repuesto { Id = 3, Name = "Repuesto3", Description = "Descripción del Repuesto3", Price = 300, StockQuantity = 30 }
        );
    }
}
