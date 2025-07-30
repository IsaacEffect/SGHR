using AutoMapper;
using SGHR.Application.DTOs.Servicios;
using SGHR.Domain.Entities.Servicios;
using ServicioCategoriaE = SGHR.Domain.Entities.Servicios.ServicioCategoria;
using ServiciosE = SGHR.Domain.Entities.Servicios.Servicios;
namespace SGHR.Application.Mapping.Servicios
{
    public class ServicioProfile : Profile
    {
        public ServicioProfile()
        {
            // Request → Entidad
            CreateMap<AgregarServicioRequest, ServiciosE>()
                .ConstructUsing(src => new ServiciosE(src.Nombre, src.Descripcion, false, null))
                .AfterMap((src, dest) =>
                {
                    if (src.Activo)
                        dest.Activar();
                    else
                        dest.Desactivar();
                });

            // Entidad → DTO
            CreateMap<ServiciosE, ServicioDto>()
                .ForMember(dest => dest.IdServicio, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Activo))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.FechaCreacion))
                .ForMember(dest => dest.FechaModificacion, opt => opt.MapFrom(src => src.FechaModificacion))
                .ForMember(dest => dest.Eliminado, opt => opt.MapFrom(src => src.Eliminado))
                .ForMember(dest => dest.FechaEliminacion, opt => opt.MapFrom(src => src.FechaEliminacion));

            // Request de actualización → Entidad
            CreateMap<ActualizarServicioRequest, ServiciosE>()
                .AfterMap((src, dest) =>
                {
                    dest.Actualizar(src.Nombre, src.Descripcion);
                    if (src.Activo)
                        dest.Activar();
                    else
                        dest.Desactivar();
                });


            CreateMap<ServicioCategoriaE, ServicioCategoriaDto>();
        }
    }
}

