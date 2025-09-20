
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ProyectoRepuestos.Helpers;

namespace ProyectoRepuestos.Models.Dtos;
public class RepuestoDto
{
    [Required(ErrorMessage = Messages.Repuesto.InvalidData)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = Messages.Repuesto.InvalidData)]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = Messages.Repuesto.InvalidData)]
    [Precision(16, 2)]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = Messages.Repuesto.InvalidData)]
    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
    public int StockQuantity { get; set; }
}