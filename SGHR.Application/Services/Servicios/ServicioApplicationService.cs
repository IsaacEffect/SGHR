using SGHR.Application.Interfaces.Servicios;
using SGHR.Application.DTOs.Servicios;

namespace SGHR.Application.Services.Servicios
{

    public class ServicioApplicationService : IServicioApplicationService
    {
        readonly IServicioApplicationService _servicioRepository;
        public ServicioApplicationService(IServicioApplicationService servicioRepository)
        {
            _servicioRepository = servicioRepository;
        }

        public async Task<ServicioDto> CrearServicioAsync(CrearServicioRequest request)
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
        public async Task ActualizarServicioAsync(ActualizarServicioRequest request)
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
        public async Task EliminarServicioAsync(int idServicio)
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
        public async Task<ServicioDto?> ObtenerServicioPorIdAsync(int idServicio)
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
        public async Task<List<ServicioDto>> ObtenerTodosLosServiciosAsync()
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
        public async Task ActivarServicioAsync(int idServicio)
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
        public async Task DesactivarServicioAsync(int idServicio)
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
        public async Task AsignarPrecioServicioCategoriaAsync(AsignarPrecioServicioCategoriaRequest request)
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
        public async Task EliminarPrecioServicioCategoriaAsync(int servicioId, int categoriaId)
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
        public async Task<List<ServicioCategoriaDto>> ObtenerPreciosServicioPorCategoriaAsync(int categoriaId)
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
        public async Task<List<ServicioCategoriaDto>> ObtenerPreciosCategoriaPorServicioAsync(int servicioId)
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
        public async Task<ServicioCategoriaDto?> ObtenerPrecioServicioCategoriaEspecificoAsync(int servicioId, int categoriaId)
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
    }
}
