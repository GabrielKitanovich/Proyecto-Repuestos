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
public class RepuestoController : BaseController<Repuesto>
{
        public RepuestoController(IRepuestoService repuestoService, IMapper mapper)
        : base(repuestoService, mapper)
    {
    }

    [HttpPost]
    public async Task<ActionResult<Repuesto>> Create(RepuestoDto newRepuesto)
    {
        var repuesto = _mapper.Map<Repuesto>(newRepuesto);
        try
        {
            var createdRepuesto = await _service.CreateAsync(repuesto);
            return CreatedAtAction(nameof(GetById), new { id = createdRepuesto.Id }, createdRepuesto);
        }
        catch (InvalidOperationException ex) when (ex.Message == Messages.Repuesto.AlreadyExists)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Repuesto>> Update(int id, RepuestoDto updatedRepuesto)
    {
        var existingRepuesto = await _service.GetByIdAsync(id);
        if (existingRepuesto == null)
            return NotFound(Messages.Repuesto.NotFound);
        try
        {
            existingRepuesto = _mapper.Map(updatedRepuesto, existingRepuesto);
            var result = await _service.UpdateAsync(id, existingRepuesto);
            return Ok(result);
        }
        catch (InvalidOperationException ex) when (ex.Message == Messages.Repuesto.AlreadyExists)
        {
            return Conflict(Messages.Repuesto.AlreadyExists);
        }

    }
}
