using AutoMapper;
using SGHR.Application.DTOs.Reservas;
using SGHR.Web.ViewModel.Reservas;

namespace SGHR.Web.ViewModel.Mapping.Reservas
{
    public class ReservasProfileApi : Profile
    {
        public ReservasProfileApi()
        {
            CreateMap<ActualizarReservaViewModel, ActualizarReservaRequest>();
            CreateMap<ReservaDto, ActualizarReservaViewModel>();
            CreateMap<ReservaDto, ReservasViewModel>();
        }

    }
}
