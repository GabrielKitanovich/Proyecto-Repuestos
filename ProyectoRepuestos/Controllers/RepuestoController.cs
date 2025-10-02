using Microsoft.AspNetCore.Mvc;
using ProyectoRepuestos.Helpers;
using ProyectoRepuestos.Models;
using ProyectoRepuestos.Models.Dtos;
using ProyectoRepuestos.Services;
using AutoMapper;
using ProyectoRepuestos.Bases;

namespace ProyectoRepuestos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RepuestoController : BaseController<Repuesto, RepuestoDto>
{
        public RepuestoController(IRepuestoService repuestoService, IMapper mapper)
        : base(repuestoService, mapper)
    {
    }
}
