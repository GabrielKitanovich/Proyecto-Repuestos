using AutoMapper;
using ProyectoRepuestos.Models;
using ProyectoRepuestos.Models.Dtos;

namespace ProyectoRepuestos.Mappers;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Repuesto, RepuestoDto>();
        CreateMap<RepuestoDto, Repuesto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());
    }
}