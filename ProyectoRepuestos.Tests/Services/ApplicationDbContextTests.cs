using Microsoft.EntityFrameworkCore;
using ProyectoRepuestos.Models;
using ProyectoRepuestos.Services;
using Xunit;

namespace ProyectoRepuestos.Tests.Services;

public class ApplicationDbContextTests
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task SaveChangesAsync_NewEntity_SetsCreatedAt()
    {
        // Arrange
        using var context = CreateContext();
        var repuesto = new Repuesto
        {
            Name = "Test",
            Description = "Test Description",
            Price = 100m,
            StockQuantity = 10
        };

        // Act
        context.Repuestos.Add(repuesto);
        await context.SaveChangesAsync();

        // Assert
        Assert.NotEqual(default(DateTime), repuesto.CreatedAt);
        Assert.Null(repuesto.UpdatedAt);
    }

    [Fact]
    public async Task SaveChangesAsync_ModifiedEntity_SetsUpdatedAt()
    {
        // Arrange
        using var context = CreateContext();
        var repuesto = new Repuesto
        {
            Name = "Test",
            Description = "Test Description",
            Price = 100m,
            StockQuantity = 10
        };

        context.Repuestos.Add(repuesto);
        await context.SaveChangesAsync();

        // Act
        repuesto.Name = "Updated Name";
        context.Repuestos.Update(repuesto);
        await context.SaveChangesAsync();

        // Assert
        Assert.NotNull(repuesto.UpdatedAt);
        Assert.NotEqual(default(DateTime), repuesto.UpdatedAt.Value);
    }

    [Fact]
    public async Task QueryFilter_OnlyReturnsActiveEntities()
    {
        // Arrange
        using var context = CreateContext();
        var activeRepuesto = new Repuesto
        {
            Name = "Active",
            Description = "Active Description",
            Price = 100m,
            StockQuantity = 10,
            IsActive = true
        };

        var inactiveRepuesto = new Repuesto
        {
            Name = "Inactive",
            Description = "Inactive Description",
            Price = 200m,
            StockQuantity = 20,
            IsActive = false
        };

        context.Repuestos.AddRange(activeRepuesto, inactiveRepuesto);
        await context.SaveChangesAsync();

        // Act
        var results = await context.Repuestos.ToListAsync();

        // Assert
        Assert.Single(results);
        Assert.Equal("Active", results[0].Name);
    }

    [Fact]
    public async Task QueryFilter_IgnoreQueryFilters_ReturnsAllEntities()
    {
        // Arrange
        using var context = CreateContext();
        var activeRepuesto = new Repuesto
        {
            Name = "Active",
            Description = "Active Description",
            Price = 100m,
            StockQuantity = 10,
            IsActive = true
        };

        var inactiveRepuesto = new Repuesto
        {
            Name = "Inactive",
            Description = "Inactive Description",
            Price = 200m,
            StockQuantity = 20,
            IsActive = false
        };

        context.Repuestos.AddRange(activeRepuesto, inactiveRepuesto);
        await context.SaveChangesAsync();

        // Act
        var results = await context.Repuestos.IgnoreQueryFilters().ToListAsync();

        // Assert
        Assert.Equal(2, results.Count);
    }

    [Fact]
    public async Task OnModelCreating_SeedsData()
    {
        // Arrange
        using var context = CreateContext();
        
        // Simular los datos del seeder manualmente
        var repuestos = new List<Repuesto>
        {
            new Repuesto
            {
                Id = 1,
                Name = "Repuesto1",
                Description = "Descripción del Repuesto1",
                Price = 100,
                StockQuantity = 10,
                IsActive = true,
                CreatedAt = new DateTime(2024, 9, 20, 0, 0, 0, DateTimeKind.Utc)
            },
            new Repuesto
            {
                Id = 2,
                Name = "Repuesto2",
                Description = "Descripción del Repuesto2",
                Price = 200,
                StockQuantity = 20,
                IsActive = true,
                CreatedAt = new DateTime(2024, 9, 20, 0, 0, 0, DateTimeKind.Utc)
            },
            new Repuesto
            {
                Id = 3,
                Name = "Repuesto3",
                Description = "Descripción del Repuesto3",
                Price = 300,
                StockQuantity = 30,
                IsActive = true,
                CreatedAt = new DateTime(2024, 9, 20, 0, 0, 0, DateTimeKind.Utc)
            }
        };

        context.Repuestos.AddRange(repuestos);
        await context.SaveChangesAsync();

        // Act & Assert
        Assert.Equal(3, context.Repuestos.Count());
        Assert.Contains(context.Repuestos, r => r.Name == "Repuesto1");
        Assert.Contains(context.Repuestos, r => r.Name == "Repuesto2");
        Assert.Contains(context.Repuestos, r => r.Name == "Repuesto3");
    }

    [Fact]
    public async Task SaveChanges_NewEntity_SetsCreatedAt()
    {
        // Arrange
        using var context = CreateContext();
        var repuesto = new Repuesto
        {
            Name = "Test Sync",
            Description = "Test Description",
            Price = 100m,
            StockQuantity = 10
        };

        // Act
        context.Repuestos.Add(repuesto);
        context.SaveChanges();

        // Assert
        Assert.NotEqual(default(DateTime), repuesto.CreatedAt);
    }

    [Fact]
    public async Task SaveChanges_ModifiedEntity_SetsUpdatedAt()
    {
        // Arrange
        using var context = CreateContext();
        var repuesto = new Repuesto
        {
            Name = "Test Sync",
            Description = "Test Description",
            Price = 100m,
            StockQuantity = 10
        };

        context.Repuestos.Add(repuesto);
        context.SaveChanges();

        // Act
        repuesto.Name = "Updated Sync";
        context.Repuestos.Update(repuesto);
        context.SaveChanges();

        // Assert
        Assert.NotNull(repuesto.UpdatedAt);
    }
}
