using Microsoft.AspNetCore.Mvc;
using ProyectoRepuestos.Helpers;
using ProyectoRepuestos.Models;
using ProyectoRepuestos.Models.Dtos;
using ProyectoRepuestos.Services;
using AutoMapper;

namespace ProyectoRepuestos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RepuestoController : ControllerBase
{
    private readonly IRepuestoService _repuestoService;
    private readonly IMapper _mapper;

    public RepuestoController(IRepuestoService repuestoService, IMapper mapper)
    {
        _repuestoService = repuestoService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Repuesto>>> GetAll()
    {
        return Ok(await _repuestoService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Repuesto>> GetById(int id)
    {
        var result = await _repuestoService.GetByIdAsync(id);
        if (result == null)
            return NotFound(Messages.Repuesto.NotFound);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Repuesto>> Create(RepuestoDto newRepuesto)
    {
        var repuesto = _mapper.Map<Repuesto>(newRepuesto);
        try
        {
            var createdRepuesto = await _repuestoService.CreateAsync(repuesto);
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
        var existingRepuesto = await _repuestoService.GetByIdAsync(id);
        if (existingRepuesto == null)
            return NotFound(Messages.Repuesto.NotFound);
        try
        {
            existingRepuesto = _mapper.Map(updatedRepuesto, existingRepuesto);
            var result = await _repuestoService.UpdateAsync(id, existingRepuesto);
            return Ok(result);
        }
        catch (InvalidOperationException ex) when (ex.Message == Messages.Repuesto.AlreadyExists)
        {
            return Conflict(Messages.Repuesto.AlreadyExists);
        }

    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _repuestoService.DeleteAsync(id);
        if (!success)
            return NotFound(Messages.Repuesto.NotFound);
        return Ok(Messages.Repuesto.Deleted + ", Id: " + id);
    }

    [HttpGet("{id}/restore")]
    public async Task<IActionResult> Restore(int id)
    {
        try
        {
            var entity = await _repuestoService.RestoreAsync(id);
            if (entity == null)
                return NotFound(Messages.Repuesto.NotFound);
    
            return Ok(entity);
        }
        catch (InvalidOperationException ex) when (ex.Message == Messages.General.AlreadyExists)
        {
            return Conflict(ex.Message);
        }
    }
}
