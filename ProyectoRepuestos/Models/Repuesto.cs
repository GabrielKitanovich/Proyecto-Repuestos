using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ProyectoRepuestos.Bases;

namespace ProyectoRepuestos.Models;
public class Repuesto : BaseModel
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Description { get; set; }
    [Required]
    [Precision(16, 2)]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive.")]
    public required decimal Price { get; set; }
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
    public required int StockQuantity { get; set; }
}