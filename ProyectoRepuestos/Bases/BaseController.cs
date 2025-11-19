using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoRepuestos.Helpers;
using ProyectoRepuestos.Logs;



namespace ProyectoRepuestos.Bases;
[Authorize]
public class BaseController<T, TDto> : ControllerBase where T : BaseModel
{
    private readonly ILogger<T> _logger;
    protected readonly IBaseService<T> _service;
    protected readonly IMapper _mapper;
    
    protected virtual string NotFoundMessage => Messages.General.NotFound;
    protected virtual string DeletedMessage => Messages.General.Deleted;


    public BaseController(IBaseService<T> service, IMapper mapper, ILogger<T> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<List<T>>>> GetAll()
    {
        _logger.GetAllEntity(typeof(T).Name, DateTime.Now);
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<T>> GetById(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null)
            return NotFound(NotFoundMessage);
        _logger.GetEntity(entity.GetType().Name, id, DateTime.Now);
        return Ok(entity);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public virtual async Task<ActionResult<T>> Create(TDto dto)
    {
        var entity = _mapper.Map<T>(dto);
        try
        {
            var createdEntity = await _service.CreateAsync(entity);
            _logger.CreatedEntity(typeof(T).Name, createdEntity.Id, DateTime.Now);
            return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
        }
        catch (InvalidOperationException ex)
        {
            _logger.OperationFailed("Create", ex.Message, typeof(T).Name, DateTime.Now);
            return Conflict(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public virtual async Task<ActionResult<T>> Update(int id, TDto dto)
    {
        var existingEntity = await _service.GetByIdAsync(id);
        if (existingEntity == null)
        {
            _logger.OperationFailed("Update", "Entity not found", typeof(T).Name, DateTime.Now);
            return NotFound(NotFoundMessage);
        }

        try
        {
            _mapper.Map(dto, existingEntity);
            var updatedEntity = await _service.UpdateAsync(id, existingEntity);
            if (_logger != null) _logger.UpdatedEntity(typeof(T).Name, updatedEntity!.Id, DateTime.Now);
            return Ok(updatedEntity);
        }
        catch (InvalidOperationException ex)
        {
            _logger.OperationFailed("Update", ex.Message, typeof(T).Name, DateTime.Now);
            return Conflict(ex.Message);
        }
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
        {
            _logger.OperationFailed("Delete", "Entity not found", typeof(T).Name, DateTime.Now);
            return NotFound(NotFoundMessage);
        }
        return Ok(DeletedMessage);
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("{id}/restore")]
    public virtual async Task<ActionResult<T?>> Restore(int id)
    {
        try
        {
            var entity = await _service.RestoreAsync(id);
            if (entity == null)
                return NotFound(NotFoundMessage);
            _logger.RestoredEntity(typeof(T).Name, entity.Id, DateTime.Now);
            return Ok(entity);
        }
        catch (InvalidOperationException ex)
        {
            _logger.OperationFailed("Restore", ex.Message, typeof(T).Name, DateTime.Now);
            return BadRequest(ex.Message);
        }
    }
}