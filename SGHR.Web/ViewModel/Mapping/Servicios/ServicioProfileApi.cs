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
            CreateMap<ServiciosViewModel, EditarServiciosViewModel>();
            CreateMap<CrearServiciosViewModel, AgregarServicioRequest>();
            CreateMap<ServiciosViewModel, ServicioConPreciosViewModel>()
            .ForMember(dest => dest.PreciosPorCategoria, opt => opt.Ignore());
            CreateMap<ServiciosViewModel, ServicioDto>();

        }
    }
}
