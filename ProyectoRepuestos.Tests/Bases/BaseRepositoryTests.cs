using Microsoft.EntityFrameworkCore;
using ProyectoRepuestos.Bases;
using ProyectoRepuestos.Models;
using ProyectoRepuestos.Services;
using Xunit;

namespace ProyectoRepuestos.Tests.Bases;

public class BaseRepositoryTests
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOnlyActiveEntities()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new BaseRepository<Repuesto>(context);
        
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

        await repository.CreateAsync(activeRepuesto);
        await context.Repuestos.AddAsync(inactiveRepuesto);
        await context.SaveChangesAsync();

        // Act
        var results = await repository.GetAllAsync();

        // Assert
        Assert.Single(results);
        Assert.Equal("Active", results[0].Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsEntity_WhenExists()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new BaseRepository<Repuesto>(context);
        
        var repuesto = new Repuesto
        {
            Name = "Test",
            Description = "Test Description",
            Price = 100m,
            StockQuantity = 10
        };

        await repository.CreateAsync(repuesto);

        // Act
        var result = await repository.GetByIdAsync(repuesto.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new BaseRepository<Repuesto>(context);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_AddsEntity()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new BaseRepository<Repuesto>(context);
        
        var repuesto = new Repuesto
        {
            Name = "New Repuesto",
            Description = "New Description",
            Price = 150m,
            StockQuantity = 15
        };

        // Act
        var result = await repository.CreateAsync(repuesto);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("New Repuesto", result.Name);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesEntity()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new BaseRepository<Repuesto>(context);
        
        var repuesto = new Repuesto
        {
            Name = "Original",
            Description = "Original Description",
            Price = 100m,
            StockQuantity = 10
        };

        await repository.CreateAsync(repuesto);

        // Act
        repuesto.Name = "Updated";
        var result = await repository.UpdateAsync(repuesto.Id, repuesto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated", result.Name);
    }

    [Fact]
    public async Task DeleteAsync_SoftDeletesEntity()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new BaseRepository<Repuesto>(context);
        
        var repuesto = new Repuesto
        {
            Name = "To Delete",
            Description = "Description",
            Price = 100m,
            StockQuantity = 10
        };

        await repository.CreateAsync(repuesto);

        // Act
        var result = await repository.DeleteAsync(repuesto.Id);

        // Assert
        Assert.True(result);
        var deletedEntity = await context.Repuestos.IgnoreQueryFilters().FirstOrDefaultAsync(r => r.Id == repuesto.Id);
        Assert.NotNull(deletedEntity);
        Assert.False(deletedEntity.IsActive);
        Assert.NotNull(deletedEntity.DeletedAt);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenEntityNotExists()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new BaseRepository<Repuesto>(context);

        // Act
        var result = await repository.DeleteAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ExistsAsync_ReturnsTrue_WhenEntityExists()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new BaseRepository<Repuesto>(context);
        
        var repuesto = new Repuesto
        {
            Name = "Exists Test",
            Description = "Description",
            Price = 100m,
            StockQuantity = 10
        };

        await repository.CreateAsync(repuesto);

        // Act
        var result = await repository.ExistsAsync(r => r.Name == "Exists Test");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_ReturnsFalse_WhenEntityNotExists()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new BaseRepository<Repuesto>(context);

        // Act
        var result = await repository.ExistsAsync(r => r.Name == "Non Existent");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task RestoreAsync_RestoresDeletedEntity()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new BaseRepository<Repuesto>(context);
        
        var repuesto = new Repuesto
        {
            Name = "To Restore",
            Description = "Description",
            Price = 100m,
            StockQuantity = 10
        };

        await repository.CreateAsync(repuesto);
        await repository.DeleteAsync(repuesto.Id);

        // Act
        var result = await repository.RestoreAsync(repuesto.Id);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsActive);
        Assert.Null(result.DeletedAt);
    }

    [Fact]
    public async Task RestoreAsync_ReturnsNull_WhenEntityNotExists()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new BaseRepository<Repuesto>(context);

        // Act
        var result = await repository.RestoreAsync(999);

        // Assert
        Assert.Null(result);
    }
}
