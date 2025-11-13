using Moq;
using ProyectoRepuestos.Bases;
using ProyectoRepuestos.Models;
using Xunit;

namespace ProyectoRepuestos.Tests.Bases;

public class BaseServiceTests
{
    private readonly Mock<IBaseRepository<Repuesto>> _mockRepository;
    private readonly BaseService<Repuesto> _service;

    public BaseServiceTests()
    {
        _mockRepository = new Mock<IBaseRepository<Repuesto>>();
        _service = new BaseService<Repuesto>(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllEntities()
    {
        // Arrange
        var repuestos = new List<Repuesto>
        {
            new Repuesto { Id = 1, Name = "Repuesto1", Description = "Desc1", Price = 100m, StockQuantity = 10 },
            new Repuesto { Id = 2, Name = "Repuesto2", Description = "Desc2", Price = 200m, StockQuantity = 20 }
        };

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(repuestos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsEntity_WhenExists()
    {
        // Arrange
        var repuesto = new Repuesto { Id = 1, Name = "Test", Description = "Desc", Price = 100m, StockQuantity = 10 };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(repuesto);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.Name);
        _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Repuesto?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(r => r.GetByIdAsync(999), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_CreatesEntity()
    {
        // Arrange
        var repuesto = new Repuesto { Name = "New", Description = "Desc", Price = 100m, StockQuantity = 10 };
        _mockRepository.Setup(r => r.CreateAsync(repuesto)).ReturnsAsync(repuesto);

        // Act
        var result = await _service.CreateAsync(repuesto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New", result.Name);
        _mockRepository.Verify(r => r.CreateAsync(repuesto), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesEntity_WhenExists()
    {
        // Arrange
        var existingRepuesto = new Repuesto { Id = 1, Name = "Original", Description = "Desc", Price = 100m, StockQuantity = 10 };
        var updatedRepuesto = new Repuesto { Id = 1, Name = "Updated", Description = "Desc", Price = 150m, StockQuantity = 15 };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingRepuesto);
        _mockRepository.Setup(r => r.UpdateAsync(1, updatedRepuesto)).ReturnsAsync(updatedRepuesto);

        // Act
        var result = await _service.UpdateAsync(1, updatedRepuesto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated", result.Name);
        _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(1, updatedRepuesto), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenNotExists()
    {
        // Arrange
        var repuesto = new Repuesto { Id = 999, Name = "Test", Description = "Desc", Price = 100m, StockQuantity = 10 };
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Repuesto?)null);

        // Act
        var result = await _service.UpdateAsync(999, repuesto);

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(r => r.GetByIdAsync(999), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<Repuesto>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_DeletesEntity_WhenExists()
    {
        // Arrange
        var repuesto = new Repuesto { Id = 1, Name = "Test", Description = "Desc", Price = 100m, StockQuantity = 10 };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(repuesto);
        _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
        _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenNotExists()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Repuesto?)null);

        // Act
        var result = await _service.DeleteAsync(999);

        // Assert
        Assert.False(result);
        _mockRepository.Verify(r => r.GetByIdAsync(999), Times.Once);
        _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task RestoreAsync_RestoresEntity_WhenDeletedExists()
    {
        // Arrange
        var repuesto = new Repuesto { Id = 1, Name = "Test", Description = "Desc", Price = 100m, StockQuantity = 10, IsActive = false };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(repuesto);
        _mockRepository.Setup(r => r.RestoreAsync(1)).ReturnsAsync(repuesto);

        // Act
        var result = await _service.RestoreAsync(1);

        // Assert
        Assert.NotNull(result);
        _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
        _mockRepository.Verify(r => r.RestoreAsync(1), Times.Once);
    }

    [Fact]
    public async Task RestoreAsync_ThrowsException_WhenEntityIsActive()
    {
        // Arrange
        var repuesto = new Repuesto { Id = 1, Name = "Test", Description = "Desc", Price = 100m, StockQuantity = 10, IsActive = true };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(repuesto);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.RestoreAsync(1));
        _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
        _mockRepository.Verify(r => r.RestoreAsync(It.IsAny<int>()), Times.Never);
    }
}
