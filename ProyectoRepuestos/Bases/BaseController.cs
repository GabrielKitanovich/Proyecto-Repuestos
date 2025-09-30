using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProyectoRepuestos.Helpers;

namespace ProyectoRepuestos.Bases;
public class BaseController<T> : ControllerBase where T : BaseModel
{
    protected readonly IBaseService<T> _service;
    protected readonly IMapper _mapper;


    public BaseController(IBaseService<T> service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<List<T>>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<T>> GetById(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null)
            return NotFound(Messages.General.NotFound);
        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(Messages.General.NotFound);
        return Ok(Messages.General.Deleted);
    }

    [HttpGet("{id}/restore")]
    public virtual async Task<ActionResult<T?>> Restore(int id)
    {
        try
        {
            var entity = await _service.RestoreAsync(id);
            if (entity == null)
                return NotFound(Messages.General.NotFound);
            return Ok(entity);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}