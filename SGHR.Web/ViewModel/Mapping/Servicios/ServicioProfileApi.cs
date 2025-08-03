using AutoMapper;
using SGHR.Application.DTOs.Servicios;
using SGHR.Web.ViewModel.Servicios;

namespace SGHR.Web.ViewModel.Mapping.Servicios
{
    public class ServicioProfileApi : Profile
    {
        public ServicioProfileApi() 
        {
            CreateMap<ServicioDto, ServiciosViewModel>();
            CreateMap<ServiciosViewModel, EditarServiciosViewModel>()
            .ForMember(dest => dest.Duracion, opt => opt.Ignore())
            .ForMember(dest => dest.Capacidad, opt => opt.Ignore())
            .ForMember(dest => dest.IdCategoriaServicio, opt => opt.Ignore());


            CreateMap<CrearServiciosViewModel, AgregarServicioRequest>();
            CreateMap<ServiciosViewModel, ServicioConPreciosViewModel>()
            .ForMember(dest => dest.PreciosPorCategoria, opt => opt.Ignore());
            CreateMap<ServiciosViewModel, ServicioDto>();
            CreateMap<AgregarServicioRequest, ServiciosViewModel>();


        }
    }
}
