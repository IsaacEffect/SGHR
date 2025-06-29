using Microsoft.EntityFrameworkCore;
using SGHR.Domain.Entities.Clientes;
using SGHR.Domain.Interfaces.Repository;
using SGHR.Persistence.Context;

public class ClienteRepository : IClienteRepository
{
    private readonly HotelReservaDBContext _context;

    public ClienteRepository(HotelReservaDBContext context)
    {
        _context = context;
    }

    public async Task<Cliente> GetByIdAsync(int id)
    {
        return await _context.Clientes.FindAsync(id);
    }

    public async Task<IEnumerable<Cliente>> GetAllAsync()
    {
        return await _context.Clientes.ToListAsync();
    }

    public async Task AddAsync(Cliente cliente)
    {
        await _context.Clientes.AddAsync(cliente);
    }

    public async Task UpdateAsync(Cliente cliente)
    {
        _context.Clientes.Update(cliente);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente != null)
        {
            _context.Clientes.Remove(cliente);
        }
    }
}
