using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoRepuestos.Bases;
public class BaseService
{
    public virtual ActionResult? EntityExists<T>(IEnumerable<T> entities, int entityId, out T? entity, string notFoundMessage = "No encontrado") where T : BaseModel
    {
        entity = entities.FirstOrDefault(e => e.Id == entityId);
        if (entity == null)
            return new NotFoundObjectResult(notFoundMessage);
        return null;
    }
}