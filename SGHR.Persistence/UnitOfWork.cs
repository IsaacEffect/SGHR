using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Domain.Base;
using SGHR.Domain.Interfaces;
using SGHR.Domain.Interfaces.Repository;
using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories;

namespace SGHR.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HotelReservaDBContext _context;
        private readonly IConfiguration _configuration;

        public IClienteRepository Clients { get; }
        public IHistorialReservaRepository HistorialReservas { get; }

        public UnitOfWork(
            HotelReservaDBContext context,
            IConfiguration configuration,
            ILogger<HistorialReservaRepository> historialLogger,
            ISqlHelper sqlHelper)
        {
            _context = context;
            _configuration = configuration;
            Clients = new ClienteRepository(_context);
            HistorialReservas = new HistorialReservaRepository(_configuration, historialLogger, sqlHelper);
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }

}
