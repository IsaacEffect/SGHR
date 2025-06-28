using Dapper;
using Microsoft.EntityFrameworkCore; 
using SGHR.Domain.Entities.Reservas;
using SGHR.Persistence.Base;
using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces; 
using SGHR.Persistence.Interfaces.Repositories.Reservas;
using System.Data;
using System; 
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks;

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

        public async Task<Reserva?> ObtenerPorIdAsync(int idReserva)
        {
            return await GetByIdAsync(idReserva);
        }

        public async Task<List<Reserva>> ObtenerReservasEnRangoAsync(DateTime desde, DateTime hasta)
        {
            return await _dbSet
                .Where(r => r.FechaEntrada >= desde && r.FechaSalida <= hasta)
                .ToListAsync();
        }

        public async Task<bool> HayDisponibilidadAsync(int habitacionId, DateTime fechaEntrada, DateTime fechaSalida)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@IdCategoriaHabitacion", habitacionId, DbType.Int32);
            parameters.Add("@FechaEntrada", fechaEntrada, DbType.Date);
            parameters.Add("@FechaSalida", fechaSalida, DbType.Date);
            parameters.Add("@EstaDisponible", dbType: DbType.Boolean, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(
                "HayDisponibilidadHabitacion",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return parameters.Get<bool>("@EstaDisponible");

        }

        public async Task ActualizarAsync(Reserva reserva)
        {
            Update(reserva);
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
        }

        public async Task CrearAsync(Reserva reserva)
        {
            await AddAsync(reserva);
        }

        
    }
}