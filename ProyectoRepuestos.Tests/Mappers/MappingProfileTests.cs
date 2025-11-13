using AutoMapper;
using Microsoft.Extensions.Logging;
using ProyectoRepuestos.Mappers;
using ProyectoRepuestos.Models;
using ProyectoRepuestos.Models.Dtos;
using Xunit;

namespace ProyectoRepuestos.Tests.Mappers;

public class MappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly MapperConfiguration _configuration;

    public MappingProfileTests()
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        _configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        }, loggerFactory);

        _mapper = _configuration.CreateMapper();
    }

    [Fact]
    public void MappingProfile_Configuration_IsValid()
    {
        // Assert
        _configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map_RepuestoToRepuestoDto_MapsCorrectly()
    {
        // Arrange
        var repuesto = new Repuesto
        {
            Name = "Filtro de Aceite",
            Description = "Filtro de alta calidad",
            Price = 25.50m,
            StockQuantity = 100,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<RepuestoDto>(repuesto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(repuesto.Name, result.Name);
        Assert.Equal(repuesto.Description, result.Description);
        Assert.Equal(repuesto.Price, result.Price);
        Assert.Equal(repuesto.StockQuantity, result.StockQuantity);
    }

    [Fact]
    public void Map_RepuestoDtoToRepuesto_MapsCorrectly()
    {
        // Arrange
        var repuestoDto = new RepuestoDto
        {
            Name = "Bujía",
            Description = "Bujía de platino",
            Price = 15.75m,
            StockQuantity = 50
        };

        // Act
        var result = _mapper.Map<Repuesto>(repuestoDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(repuestoDto.Name, result.Name);
        Assert.Equal(repuestoDto.Description, result.Description);
        Assert.Equal(repuestoDto.Price, result.Price);
        Assert.Equal(repuestoDto.StockQuantity, result.StockQuantity);
    }

    [Fact]
    public void Map_RepuestoToRepuestoDto_WithNullValues_MapsCorrectly()
    {
        // Arrange
        var repuesto = new Repuesto
        {
            Id = 1,
            Name = "Test",
            Description = null,
            Price = 0,
            StockQuantity = 0
        };

        // Act
        var result = _mapper.Map<RepuestoDto>(repuesto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(repuesto.Name, result.Name);
        Assert.Null(result.Description);
        Assert.Equal(0, result.Price);
        Assert.Equal(0, result.StockQuantity);
    }

    [Fact]
    public void Map_RepuestoDtoToRepuesto_WithNullValues_MapsCorrectly()
    {
        // Arrange
        var repuestoDto = new RepuestoDto
        {
            Name = "Test",
            Description = null,
            Price = 0,
            StockQuantity = 0
        };

        // Act
        var result = _mapper.Map<Repuesto>(repuestoDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(repuestoDto.Name, result.Name);
        Assert.Null(result.Description);
        Assert.Equal(0, result.Price);
        Assert.Equal(0, result.StockQuantity);
    }
}
