using System.ComponentModel.DataAnnotations;

namespace ProyectoRepuestos.Bases;

public abstract class BaseModel
{

    public int Id { get; set; }
    public bool IsActive { get; set; } = true;

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? UpdatedAt { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? DeletedAt { get; set; }
}
