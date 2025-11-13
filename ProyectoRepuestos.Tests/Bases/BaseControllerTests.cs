using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProyectoRepuestos.Bases;
using ProyectoRepuestos.Controllers;
using ProyectoRepuestos.Helpers;
using ProyectoRepuestos.Models;
using ProyectoRepuestos.Models.Dtos;
using ProyectoRepuestos.Services;
using Xunit;

namespace ProyectoRepuestos.Tests.Bases;

public class BaseControllerTests
{
    private readonly Mock<IRepuestoService> _mockService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly RepuestoController _controller;

    public BaseControllerTests()
    {
        _mockService = new Mock<IRepuestoService>();
        _mockMapper = new Mock<IMapper>();
        _controller = new RepuestoController(_mockService.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithListOfEntities()
    {
        // Arrange
        var repuestos = new List<Repuesto>
        {
            new Repuesto { Id = 1, Name = "Test1", Description = "Desc1", Price = 100m, StockQuantity = 10 },
            new Repuesto { Id = 2, Name = "Test2", Description = "Desc2", Price = 200m, StockQuantity = 20 }
        };

        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(repuestos);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedList = Assert.IsAssignableFrom<List<Repuesto>>(okResult.Value);
        Assert.Equal(2, returnedList.Count);
    }

    [Fact]
    public async Task GetById_ReturnsOkResult_WhenEntityExists()
    {
        // Arrange
        var repuesto = new Repuesto { Id = 1, Name = "Test", Description = "Desc", Price = 100m, StockQuantity = 10 };
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(repuesto);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedRepuesto = Assert.IsType<Repuesto>(okResult.Value);
        Assert.Equal("Test", returnedRepuesto.Name);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenEntityDoesNotExist()
    {
        // Arrange
        _mockService.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((Repuesto?)null);

        // Act
        var result = await _controller.GetById(999);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(Messages.Repuesto.NotFound, notFoundResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WhenSuccessful()
    {
        // Arrange
        var dto = new RepuestoDto { Name = "New", Description = "New Desc", Price = 100m, StockQuantity = 10 };
        var repuesto = new Repuesto { Id = 1, Name = "New", Description = "New Desc", Price = 100m, StockQuantity = 10 };

        _mockMapper.Setup(m => m.Map<Repuesto>(dto)).Returns(repuesto);
        _mockService.Setup(s => s.CreateAsync(repuesto)).ReturnsAsync(repuesto);

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(_controller.GetById), createdResult.ActionName);
        var returnedRepuesto = Assert.IsType<Repuesto>(createdResult.Value);
        Assert.Equal("New", returnedRepuesto.Name);
    }

    [Fact]
    public async Task Create_ReturnsConflict_WhenEntityAlreadyExists()
    {
        // Arrange
        var dto = new RepuestoDto { Name = "Existing", Description = "Desc", Price = 100m, StockQuantity = 10 };
        var repuesto = new Repuesto { Name = "Existing", Description = "Desc", Price = 100m, StockQuantity = 10 };

        _mockMapper.Setup(m => m.Map<Repuesto>(dto)).Returns(repuesto);
        _mockService.Setup(s => s.CreateAsync(repuesto))
            .ThrowsAsync(new InvalidOperationException(Messages.Repuesto.AlreadyExists));

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
        Assert.Equal(Messages.Repuesto.AlreadyExists, conflictResult.Value);
    }

    [Fact]
    public async Task Update_ReturnsOkResult_WhenSuccessful()
    {
        // Arrange
        var dto = new RepuestoDto { Name = "Updated", Description = "Updated Desc", Price = 150m, StockQuantity = 15 };
        var existingRepuesto = new Repuesto { Id = 1, Name = "Original", Description = "Original Desc", Price = 100m, StockQuantity = 10 };
        var updatedRepuesto = new Repuesto { Id = 1, Name = "Updated", Description = "Updated Desc", Price = 150m, StockQuantity = 15 };

        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(existingRepuesto);
        _mockMapper.Setup(m => m.Map(dto, existingRepuesto)).Returns(updatedRepuesto);
        _mockService.Setup(s => s.UpdateAsync(1, existingRepuesto)).ReturnsAsync(updatedRepuesto);

        // Act
        var result = await _controller.Update(1, dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedRepuesto = Assert.IsType<Repuesto>(okResult.Value);
        Assert.Equal("Updated", returnedRepuesto.Name);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenEntityDoesNotExist()
    {
        // Arrange
        var dto = new RepuestoDto { Name = "Updated", Description = "Desc", Price = 100m, StockQuantity = 10 };
        _mockService.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((Repuesto?)null);

        // Act
        var result = await _controller.Update(999, dto);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(Messages.Repuesto.NotFound, notFoundResult.Value);
    }

    [Fact]
    public async Task Update_ReturnsConflict_WhenNameAlreadyExists()
    {
        // Arrange
        var dto = new RepuestoDto { Name = "Existing", Description = "Desc", Price = 100m, StockQuantity = 10 };
        var existingRepuesto = new Repuesto { Id = 1, Name = "Original", Description = "Desc", Price = 100m, StockQuantity = 10 };

        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(existingRepuesto);
        _mockMapper.Setup(m => m.Map(dto, existingRepuesto)).Returns(existingRepuesto);
        _mockService.Setup(s => s.UpdateAsync(1, existingRepuesto))
            .ThrowsAsync(new InvalidOperationException(Messages.Repuesto.AlreadyExists));

        // Act
        var result = await _controller.Update(1, dto);

        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
        Assert.Equal(Messages.Repuesto.AlreadyExists, conflictResult.Value);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(Messages.Repuesto.Deleted, okResult.Value);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenEntityDoesNotExist()
    {
        // Arrange
        _mockService.Setup(s => s.DeleteAsync(999)).ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(999);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(Messages.Repuesto.NotFound, notFoundResult.Value);
    }

    [Fact]
    public async Task Restore_ReturnsOkResult_WhenSuccessful()
    {
        // Arrange
        var repuesto = new Repuesto { Id = 1, Name = "Restored", Description = "Desc", Price = 100m, StockQuantity = 10, IsActive = true };
        _mockService.Setup(s => s.RestoreAsync(1)).ReturnsAsync(repuesto);

        // Act
        var result = await _controller.Restore(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedRepuesto = Assert.IsType<Repuesto>(okResult.Value);
        Assert.Equal("Restored", returnedRepuesto.Name);
        Assert.True(returnedRepuesto.IsActive);
    }

    [Fact]
    public async Task Restore_ReturnsNotFound_WhenEntityDoesNotExist()
    {
        // Arrange
        _mockService.Setup(s => s.RestoreAsync(999)).ReturnsAsync((Repuesto?)null);

        // Act
        var result = await _controller.Restore(999);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(Messages.Repuesto.NotFound, notFoundResult.Value);
    }

    [Fact]
    public async Task Restore_ReturnsBadRequest_WhenEntityIsAlreadyActive()
    {
        // Arrange
        _mockService.Setup(s => s.RestoreAsync(1))
            .ThrowsAsync(new InvalidOperationException(Messages.General.AlreadyExists));

        // Act
        var result = await _controller.Restore(1);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(Messages.General.AlreadyExists, badRequestResult.Value);
    }
}
