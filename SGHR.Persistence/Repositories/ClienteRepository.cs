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
        return await _context.Clientes
        .Where(c => c.Id == id && c.Estado && c.Rol == "Cliente")
        .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Cliente>> GetAllAsync()
    {
        return await _context.Clientes
        .Where(c => c.Estado && c.Rol == "Cliente")
        .ToListAsync();
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
            cliente.Estado = false;
            _context.Clientes.Update(cliente);
        }
    }

    public async Task<Cliente> GetByEmailAsync(string email)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.Correo.ToLower() == email.ToLower());
    }
}