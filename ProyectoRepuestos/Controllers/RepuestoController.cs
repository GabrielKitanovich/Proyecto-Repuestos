using Microsoft.AspNetCore.Mvc;
using ProyectoRepuestos.Helpers;
using ProyectoRepuestos.Models;
using ProyectoRepuestos.Services;

namespace ProyectoRepuestos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RepuestoController(RepuestoService repuestoService) : ControllerBase
{
    private readonly RepuestoService _repuestoService = repuestoService;

    [HttpGet]
    public ActionResult<IEnumerable<Repuesto>> GetAll()
    {
        return Ok(_repuestoService.Repuestos);
    }

    [HttpGet("{id}")]
    public ActionResult<Repuesto> GetById(int id)
    {
        ActionResult? result = _repuestoService.RepuestoExists(id, out Repuesto? repuesto);
        if (result != null)
            return result;
        return Ok(repuesto);
    }

    ///[HttpPost]
    ///public ActionResult<Repuesto> Create(Repuesto newRepuesto)
}
