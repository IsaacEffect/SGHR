using SGHR.Application.Interfaces.Servicios;
using SGHR.Persistence.Interfaces.Repositories.Servicios;
namespace SGHR.Application.Validations
{
    public class ServicioRules(IServicioRepository servicioRepository) : IServicioRules
    {
        private readonly IServicioRepository _servicioRepository = servicioRepository;

        public async Task ValidarDatosBasicosAsync(string nombre, string descripcion)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del servicio no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("La descripción del servicio no puede estar vacía.");

            await Task.CompletedTask;
        }
        public async Task ValidarExistenciaSerivicioAsync(int idServicio)
        {
            _ = await _servicioRepository.ObtenerPorIdAsync(idServicio)
                 ?? throw new KeyNotFoundException($"El servicio con ID {idServicio} no fue encontrado");
        }
        public void ValidarIdPositivo(int idServicio)
        {
            if (idServicio <= 0)
            {
                throw new ArgumentException("El ID del servicio debe ser un número positivo.");
            }
        }
        public void ValidarPrecioServicio(decimal precio)
        {
            if (precio < 0)
            {
                throw new ArgumentException("El precio del servicio no puede ser negativo.");
            }
        }
    }
}
