using ProyectoRepuestos.Helpers;

namespace ProyectoRepuestos.Bases;

public class BaseService<T> : IBaseService<T> where T : BaseModel
{
    protected readonly IBaseRepository<T> _repository;
    public BaseService(IBaseRepository<T> repository)
    {
        _repository = repository;
    }
    public async Task<T> CreateAsync(T entity)
    {
        await _repository.CreateAsync(entity);
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            return false;
        await _repository.DeleteAsync(id);
        return true;
    }


    public async Task<List<T>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {

        return await _repository.GetByIdAsync(id);
    }

    public async Task<T?> RestoreAsync(int id)
    {
        return await _repository.RestoreAsync(id);
    }

    public async Task<T?> UpdateAsync(int id, T entity)
    {
        var existingEntity = await _repository.GetByIdAsync(id);
        if (existingEntity != null)
        {
            return await _repository.UpdateAsync(id, entity);
        }
        return existingEntity;
    }
}