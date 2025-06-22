using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Domain.Interfaces;
using SGHR.Domain.Repository;
using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ILogger<HistorialReservaRepository> historialLogger)
        {
            _context = context;
            _configuration = configuration;
            Clients = new ClienteRepository(_context);
            HistorialReservas = new HistorialReservaRepository(_configuration, historialLogger);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }

}
