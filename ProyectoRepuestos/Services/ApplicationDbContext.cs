using Microsoft.EntityFrameworkCore;
using ProyectoRepuestos.Bases;
using ProyectoRepuestos.Models;
namespace ProyectoRepuestos.Services;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Repuesto> Repuestos { get; set; }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseModel>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<BaseModel>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChanges();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Repuesto>().HasData(
            new Repuesto
            {
                Id = 1,
                Name = "Repuesto1",
                Description = "Descripción del Repuesto1",
                Price = 100,
                StockQuantity = 10,
                IsActive = true,
                CreatedAt = new DateTime(2024, 9, 20, 0, 0, 0, DateTimeKind.Utc) // Valor estático
            },
            new Repuesto
            {
                Id = 2,
                Name = "Repuesto2",
                Description = "Descripción del Repuesto2",
                Price = 200,
                StockQuantity = 20,
                IsActive = true,
                CreatedAt = new DateTime(2024, 9, 20, 0, 0, 0, DateTimeKind.Utc) // Valor estático
            },
            new Repuesto
            {
                Id = 3,
                Name = "Repuesto3",
                Description = "Descripción del Repuesto3",
                Price = 300,
                StockQuantity = 30,
                IsActive = true,
                CreatedAt = new DateTime(2024, 9, 20, 0, 0, 0, DateTimeKind.Utc) // Valor estático
            }
        );
    }
}
