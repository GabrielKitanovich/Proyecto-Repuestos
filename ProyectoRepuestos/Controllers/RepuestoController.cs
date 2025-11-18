using Microsoft.AspNetCore.Mvc;
using ProyectoRepuestos.Models;
using ProyectoRepuestos.Models.Dtos;
using ProyectoRepuestos.Services;
using AutoMapper;
using ProyectoRepuestos.Bases;
using ProyectoRepuestos.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoRepuestos.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RepuestoController : BaseController<Repuesto, RepuestoDto>
{
    public RepuestoController(IRepuestoService repuestoService, IMapper mapper, ILogger<Repuesto> logger)
    : base(repuestoService, mapper, logger)
    {
    }

    protected override string NotFoundMessage => Messages.Repuesto.NotFound;
    protected override string DeletedMessage => Messages.Repuesto.Deleted;
}
