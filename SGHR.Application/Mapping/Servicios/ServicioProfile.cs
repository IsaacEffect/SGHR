using SGHR.Application.DTOs.Servicios;
using ServiciosE = SGHR.Domain.Entities.Servicios.Servicios;
using ServicioCategoriaE = SGHR.Domain.Entities.Servicios.ServicioCategoria;
using AutoMapper;
namespace SGHR.Application.Mapping.Servicios
{
    public class ServicioProfile : Profile
    {
        public ServicioProfile()
        {
            CreateMap<AgregarServicioRequest, ServiciosE>()
                .ConstructUsing(src => new ServiciosE(src.Nombre, src.Descripcion))
                .AfterMap((src, dest) =>
                {
                    if (src.Activo)
                        dest.Activar();
                    else
                        dest.Desactivar();
                });

            CreateMap<ServiciosE, ServicioDto>()
                .ForMember(dest => dest.IdServicio, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Activo));

            CreateMap<ActualizarServicioRequest, ServiciosE>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdServicio))
                .AfterMap((src, dest) =>
                {
                    if (src.Activo)
                        dest.Activar();
                    else
                        dest.Desactivar();
                });

            CreateMap<ServicioCategoriaE, ServicioCategoriaDto>().ReverseMap();


        }
    }
}

