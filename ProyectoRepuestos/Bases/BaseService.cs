using ProyectoRepuestos.Helpers;

namespace ProyectoRepuestos.Bases;

public class BaseService<T> : IBaseService<T> where T : BaseModel
{
    protected readonly IBaseRepository<T> _repository;
    public BaseService(IBaseRepository<T> repository)
    {
        _repository = repository;
    }
    public virtual async Task<T> CreateAsync(T entity)
    {
        await _repository.CreateAsync(entity);
        return entity;
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            return false;
        await _repository.DeleteAsync(id);
        return true;
    }


    public virtual async Task<List<T>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {

        return await _repository.GetByIdAsync(id);
    }

    public virtual async Task<T?> RestoreAsync(int id)
    {
        var existingEntity = await _repository.GetByIdAsync(id);
        if (existingEntity != null && existingEntity.IsActive)
        {
            throw new InvalidOperationException(Messages.Repuesto.AlreadyExists);
        }

        return await _repository.RestoreAsync(id);
    }

    public virtual async Task<T?> UpdateAsync(int id, T entity)
    {
        var existingEntity = await _repository.GetByIdAsync(id);
        if (existingEntity != null)
        {
            return await _repository.UpdateAsync(id, entity);
        }
        return existingEntity;
    }
}