using SGHR.Domain.Base;
using SGHR.Domain.enums;    
namespace SGHR.Domain.Entities.Reservas
{

    public class Reserva : EntityBase
    {
        public int ClienteId { get; private set; }
        public int IdCategoriaHabitacion { get; private set; }
        public DateTime FechaEntrada { get; private set; }
        public DateTime FechaSalida { get; private set; }
        public EstadoReserva Estado { get; private set; }
        public DateTime FechaCreacion { get; private set; }


        public Reserva(int reservaId, int clienteId, int idCategoriaHabitacion, DateTime entrada, DateTime salida, int estado)
        {
            if (entrada.Date < DateTime.Today || salida <= entrada)
                throw new ArgumentException("Las fechas de la reserva no son válidas.");

            
            ClienteId = clienteId;
            IdCategoriaHabitacion = idCategoriaHabitacion;
            FechaEntrada = entrada;
            FechaSalida = salida;
            Estado = EstadoReserva.Confirmada;
            FechaCreacion = DateTime.UtcNow;
            
        }

        protected Reserva() {}

        public void Cancelar()
        {
            if (Estado != EstadoReserva.Confirmada)
                throw new InvalidOperationException("Solo se pueden cancelar reservas confirmadas.");
            Estado = EstadoReserva.Cancelada;
        }
    }
}


