using Microsoft.AspNetCore.Mvc;
using ProyectoRepuestos.Helpers;
using ProyectoRepuestos.Models;

namespace ProyectoRepuestos.Services;

public class RepuestoService : Bases.BaseService
{
    public List<Repuesto> Repuestos { get; set; }

    public ActionResult? RepuestoExists(int repuestoId, out Repuesto? repuesto)
    {
        return EntityExists(Repuestos, repuestoId, out repuesto, Messages.Repuesto.NotFound);
    }
}
