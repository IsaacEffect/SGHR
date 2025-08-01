using SGHR.Web.ApiRepositories.Base;
using SGHR.Web.ApiRepositories.Interfaces.Servicios;
using SGHR.Web.Models;
using SGHR.Web.ViewModel.ServicioCategoria;

namespace SGHR.Web.ApiRepositories
{
    public class ServicioCategoriaApiRepository : HttpServiceBase, IServicioCategoriaApiRepository
    {
        private const string _baseEndpoint = "api/ServicioCategoria";

        public ServicioCategoriaApiRepository(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResponse<object>> AsignarActualizarPrecioAsync(AsignarPrecioServicioCategoriaViewModel Model)
        {
            return PostAsync<object>($"{_baseEndpoint}/AsignarPrecio", Model);
        }

        public Task<ApiResponse<List<ServicioCategoriaViewModel>>> ObtenerPreciosPorServicioAsync(int idServicio)
        {
            return GetListAsync<ServicioCategoriaViewModel>($"{_baseEndpoint}/ObtenerPreciosCategoriaPorServicio/{idServicio}");
        }

        public Task<ApiResponse<ServicioCategoriaViewModel>> ObtenerPrecioEspecificoAsync(int idServicio, int idCategoriaHabitacion)
        {
            var endpoint = $"{_baseEndpoint}/ObtenerPrecioServicioCategoriaEspecifico?idServicio={idServicio}&idCategoriaHabitacion={idCategoriaHabitacion}";
            return GetAsync<ServicioCategoriaViewModel>(endpoint);
        }
    }
}