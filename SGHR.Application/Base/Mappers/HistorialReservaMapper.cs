using SGHR.Application.Dtos;
using SGHR.Domain.Entities.Historial;

namespace SGHR.Application.Base.Mappers
{
    public static class HistorialReservaMapper
    {
        public static HistorialReservaDto ToDto(this HistorialReserva entidad)
        {
            return new HistorialReservaDto
            {
                FechaEntrada = entidad.FechaEntrada,
                FechaSalida = entidad.FechaSalida,
                Estado = entidad.Estado,
                Tarifa = entidad.Tarifa,
                TipoHabitacion = entidad.TipoHabitacion,
                ServiciosAdicionales = entidad.ServiciosAdicionales
            };
        }

        public static IEnumerable<HistorialReservaDto> ToDtoList(this IEnumerable<HistorialReserva> historial)
        {
            return historial.Select(h => h.ToDto());
        }
    }
}
