using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos
{
    public record HistorialReservaDto
    {
        public int Id { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }

        [StringLength(30)]
        public string Estado { get; set; }
        public decimal Tarifa { get; set; }

        [StringLength(50)]
        public string TipoHabitacion { get; set; }
        public string ServiciosAdicionales { get; set; }
    }
}
