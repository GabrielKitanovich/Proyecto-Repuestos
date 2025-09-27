namespace ProyectoRepuestos.Bases
{
    public interface IBaseService<T> where T : BaseModel
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task<T?> UpdateAsync(int id, T entity);
        Task<bool> DeleteAsync(int id);
        Task<T?> RestoreAsync(int id);
    }
}