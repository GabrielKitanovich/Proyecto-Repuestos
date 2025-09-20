using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoRepuestos.Bases
{
    public interface IBaseService
    {
        Task<List<T>> GetAllAsync<T>() where T : BaseModel;
        Task<T?> GetByIdAsync<T>(int id) where T : BaseModel;
        Task<T> CreateAsync<T>(T entity) where T : BaseModel;
        Task<T?> UpdateAsync<T>(int id, T entity) where T : BaseModel;
        Task<bool> DeleteAsync<T>(int id) where T : BaseModel;
    }
}