using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SGHR.Application.DTOs.Reservas;
using SGHR.Domain.Entities.Reservas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.Mapping.Reservas
{
    public class ReservaProfile : Profile
    {
        public ReservaProfile() 
        {
            CreateMap<CrearReservaRequest, Reserva>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.NumeroReservaUnico, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCancelacion, opt => opt.Ignore())
                .ForMember(dest => dest.MotivoCancelacion, opt => opt.Ignore());

            CreateMap<Reserva, ReservaDto>()
                .ForMember(dest => dest.IdCliente, opt => opt.MapFrom(src => src.ClienteId))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            
            CreateMap<ActualizarReservaRequest, Reserva>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCancelacion, opt => opt.Ignore())
                .ForMember(dest => dest.MotivoCancelacion, opt => opt.Ignore());

            CreateMap<VerificarDisponibilidadRequest, Reserva>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.Ignore());

            CreateMap<CancelarReservaDto, Reserva>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.MotivoCancelacion, opt => opt.Ignore());
        }

    }
    
}