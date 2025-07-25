using SGHR.Domain.Base;
using SGHR.Domain.Entities.Habitaciones;
using SGHR.Domain.Entities.Users;
using SGHR.Domain.Enums;
namespace SGHR.Domain.Entities.Reservas
{

    public class Reserva : AuditableEntity
    {
        public int Id { get; protected set; }
        public int ClienteId { get; private set; }
        public int IdCategoriaHabitacion { get; private set; }
        public DateTime FechaEntrada { get; private set; }
        public DateTime FechaSalida { get; private set; }
        public EstadoReserva Estado { get; private set; }
        public int NumeroHuespedes { get; private set; } = 1;
        public string NumeroReservaUnico { get; private set; } 
        public DateTime? FechaCancelacion { get; private set; }
        public string? MotivoCancelacion { get; private set; }
        public Cliente? Cliente { get; private set; }
        public CategoriaHabitacion? CategoriaHabitacion { get; private set; }

        protected Reserva() { }
        public Reserva(int clienteId, int idCategoriaHabitacion, DateTime fechaEntrada, DateTime fechaSalida, int numeroHuespedes)
        {
            if (clienteId <= 0)
            {
                throw new ArgumentException("El ID del cliente debe ser un número positivo.", nameof(clienteId));
            }
            if (idCategoriaHabitacion <= 0)
            {
                throw new ArgumentException("El ID de la categoría de habitación debe ser un número positivo.", nameof(idCategoriaHabitacion));
            }
            if (fechaEntrada > fechaSalida)
            {
                throw new ArgumentException("La fecha de entrada no puede ser posterior a la fecha de salida.");
            }
            if (numeroHuespedes <= 0)
            {
                throw new ArgumentException("El número de huéspedes debe ser mayor que cero.");
            }

            ClienteId = clienteId;
            IdCategoriaHabitacion = idCategoriaHabitacion;
            FechaEntrada = fechaEntrada;
            FechaSalida = fechaSalida;
            NumeroHuespedes = numeroHuespedes; 
            Estado = EstadoReserva.Pendiente;

            NumeroReservaUnico = GenerarNumeroReservaUnico();
            FechaCreacion = DateTime.Now;


        }

        public string GenerarNumeroReservaUnico()
        {
            return $"RSV-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
        }
       
        public void ActualizarDetalles(int clienteId, int idCategoriaHabitacion, DateTime fechaEntrada, DateTime fechaSalida, int numeroHuespedes)
        {
            if (clienteId <= 0)
            {
                throw new ArgumentException("El ID del cliente debe ser un número positivo.", nameof(clienteId));
            }
            if (idCategoriaHabitacion <= 0)
            {
                throw new ArgumentException("El ID de la categoría de habitación debe ser un número positivo.", nameof(idCategoriaHabitacion));
            }
            if (fechaEntrada > fechaSalida)
            {
                throw new ArgumentException("La fecha de entrada no puede ser posterior a la fecha de salida.", nameof(fechaEntrada));
            }
            if (numeroHuespedes <= 0)
            {
                throw new ArgumentException("El número de huéspedes debe ser mayor que cero.", nameof(numeroHuespedes));
            }
            
            ClienteId = clienteId;
            IdCategoriaHabitacion = idCategoriaHabitacion;
            FechaEntrada = fechaEntrada;
            FechaSalida = fechaSalida;


            SetFechaUltimaModificacion();
        }
        public void Confirmar()
        {
            if (Estado != EstadoReserva.Pendiente)
                throw new InvalidOperationException("Solo se pueden confirmar reservas pendientes.");
            Estado = EstadoReserva.Confirmada;

            SetFechaUltimaModificacion();
        }


        public void Cancelar()
        {
            if (Estado != EstadoReserva.Confirmada)
                throw new InvalidOperationException("Solo se pueden cancelar reservas confirmadas.");
            Estado = EstadoReserva.Cancelada;
            Desactivar();
            SetFechaUltimaModificacion();
        }

        public void Finalizar()
        {
            if (Estado != EstadoReserva.Confirmada)
                throw new InvalidOperationException("Solo se pueden finalizar reservas confirmadas.");
            Estado = EstadoReserva.Finalizada;
            SetFechaUltimaModificacion();

        }
    }
}