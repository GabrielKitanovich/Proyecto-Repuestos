using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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

    /*    public virtual ActionResult? EntityExists<T>(IEnumerable<T> entities, int entityId, out T? entity, string notFoundMessage = "No encontrado") where T : BaseModel
    {
        entity = entities.FirstOrDefault(e => e.Id == entityId);
        if (entity == null)
            return new NotFoundObjectResult(notFoundMessage);
        return null;
    } */

}