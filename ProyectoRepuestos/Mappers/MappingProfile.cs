using AutoMapper;
using ProyectoRepuestos.Models;
using ProyectoRepuestos.Models.Dtos;

namespace ProyectoRepuestos.Mappers;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Repuesto, RepuestoDto>();
        CreateMap<RepuestoDto, Repuesto>();
    }
}