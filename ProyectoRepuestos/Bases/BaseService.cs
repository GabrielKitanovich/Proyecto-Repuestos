namespace ProyectoRepuestos.Bases;

public class BaseService : IBaseService
{
    public Task<T> CreateAsync<T>(T entity) where T : BaseModel
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync<T>(int id) where T : BaseModel
    {
        throw new NotImplementedException();
    }


    public Task<List<T>> GetAllAsync<T>() where T : BaseModel
    {
        throw new NotImplementedException();
    }

    public Task<T?> GetByIdAsync<T>(int id) where T : BaseModel
    {
        throw new NotImplementedException();
    }

    public Task<T?> UpdateAsync<T>(int id, T entity) where T : BaseModel
    {
        throw new NotImplementedException();
    }

}