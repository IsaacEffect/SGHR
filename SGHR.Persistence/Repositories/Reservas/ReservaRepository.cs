using Dapper;
using Microsoft.EntityFrameworkCore; 
using SGHR.Domain.Entities.Reservas;
using SGHR.Persistence.Base;
using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces; 
using SGHR.Persistence.Interfaces.Repositories.Reservas;
using System.Data;
using SGHR.Domain.Enums;

namespace SGHR.Persistence.Repositories.Reservas
{
    
    public class ReservaRepository : BaseRepository<Reserva>, IReservaRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public ReservaRepository(SGHRDbContext context, ISqlConnectionFactory sqlConnectionFactory) : base(context)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }
        public async Task<List<Reserva>> ObtenerPorClienteIdAsync(int idCliente)
        {
            return await _dbSet
                .Where(r => r.ClienteId == idCliente)
                .ToListAsync();
        }
        public async Task<List<Reserva>> ObtenerTodasAsync(bool IncluirRelaciones = false)
        {
            if (IncluirRelaciones)
            {
                return await _dbSet
                    .Include(r => r.Cliente)
                    .Include(r => r.CategoriaHabitacion)
                    .ToListAsync();
            }
            return (await GetAllAsync()).ToList(); 
        }

        public async Task<List<Reserva>> ObtenerReservasEnRangoAsync(DateTime desde, DateTime hasta)
        {
            return await _dbSet
                .Where(r => r.FechaEntrada.Date >= desde.Date && r.FechaSalida.Date <= hasta.Date)
                .ToListAsync();
        }

        public async Task<bool> HayDisponibilidadAsync(int IdCategoriaHabitacion, DateTime fechaEntrada, DateTime fechaSalida, int? idReservaActual = null)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@IdCategoriaHabitacion", IdCategoriaHabitacion, DbType.Int32);
            parameters.Add("@FechaEntrada", fechaEntrada, DbType.Date);
            parameters.Add("@FechaSalida", fechaSalida, DbType.Date);
            parameters.Add("@EstaDisponible", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@IdReservaActual", idReservaActual, DbType.Int32);
            await connection.ExecuteAsync(
                "HayDisponibilidadHabitacion",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return parameters.Get<bool>("@EstaDisponible");
        }

        public async Task CrearReservaAsync(Reserva reserva)
        {
            await _dbSet.AddAsync(reserva);
        }

        public Task ActualizarReservaAsync(Reserva reserva)
        {
            Update(reserva);
            return Task.CompletedTask;
        }

        public async Task CancelarReservaAsync(int idReserva)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@IdReserva", idReserva, DbType.Int32);
            parameters.Add("@FechaCancelacion", DateTime.Now, DbType.DateTime);
            parameters.Add("@MotivosCancelacion", "Cancelada desde la aplicacion", DbType.String);

            await connection.ExecuteAsync(
                "CancelarReserva",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var reserva = await _dbSet.FindAsync(idReserva);
            if (reserva != null)
            {
                reserva.ActualizarEstado(EstadoReserva.Cancelada);
                reserva.ActualizarMotivoCancelacion("Cancelada desde la aplicacion");
                _context.Update(reserva);
            }
        }
    }
}