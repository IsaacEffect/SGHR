using SGHR.Web.ApiRepositories.Base;
using SGHR.Web.ApiRepositories.Interfaces.Servicios;
using SGHR.Web.Models;
using SGHR.Web.ViewModel.ServicioCategoria;

namespace SGHR.Web.ApiRepositories.Servicios
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

    }
}