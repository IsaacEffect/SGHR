using SGHR.Domain.Entities.Servicios;
using SGHR.Domain.Enums;

namespace SGHR.Domain.Entities.Reservas
{

    public class Reserva
    {
        public int IdReserva { get; private set; }
        public int ClienteId { get; private set; }
        public int IdCategoriaHabitacion { get; private set; }
        public DateTime FechaEntrada { get; private set; }
        public DateTime FechaSalida { get; private set; }
        public EstadoReserva Estado { get; private set; }
        public DateTime FechaCreacion { get; private set; }
//        public List<ServicioReserva> ServiciosAdicionales { get; private set; }

        public Reserva(int reservaId, int clienteId, int idCategoriaHabitacion, DateTime entrada, DateTime salida)
        {
            if (entrada.Date < DateTime.Today || salida <= entrada)
                throw new ArgumentException("Las fechas de la reserva no son válidas.");

            IdReserva = reservaId; 
            ClienteId = clienteId;
            IdCategoriaHabitacion = idCategoriaHabitacion;
            FechaEntrada = entrada;
            FechaSalida = salida;
            Estado = EstadoReserva.Confirmada;
            FechaCreacion = DateTime.UtcNow;
            // ServiciosAdicionales = new List<ServicioReserva>();
        }

        public void Cancelar()
        {
            if (Estado == EstadoReserva.Cancelada)
                throw new InvalidOperationException("La reserva ya fue cancelada.");
            Estado = EstadoReserva.Cancelada;
        }

    //    public void AgregarServicio(ServicioReserva servicio)
    //    {
    //        if (Estado == EstadoReserva.Cancelada)
    //            throw new InvalidOperationException("No se pueden agregar servicios a una reserva cancelada.");

    //        ServiciosAdicionales.Add(servicio);
    //    }
    }
}


