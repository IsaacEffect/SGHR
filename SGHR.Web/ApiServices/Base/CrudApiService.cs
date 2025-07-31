using SGHR.Web.ApiServices.Interfaces;
using SGHR.Web.ViewModel;

namespace SGHR.Web.ApiServices.Base
{
    public abstract class CrudApiService<TEntity, TCreateModel, TUpdateModel>: HttpServiceBase, ICrudApiService<TEntity,TCreateModel,TUpdateModel>
        where TEntity : class
        where TCreateModel : class
        where TUpdateModel : class
    {
        protected readonly string _baseEndpoint;

        public CrudApiService(HttpClient httpClient, string baseEndpoint)
            : base(httpClient)
        {
            _baseEndpoint = baseEndpoint;
        }

        public virtual Task<ApiResponse<List<TEntity>>> GetAllAsync()
        {
            return GetListAsync<TEntity>(_baseEndpoint);
        }

        public virtual Task<ApiResponse<TUpdateModel>> GetByIdAsync(int id)
        {
            var endpoint = $"{_baseEndpoint}/{id}";
            return GetAsync<TUpdateModel>(endpoint);
        }

        public virtual Task<ApiResponse<TEntity>> CreateAsync(TCreateModel model)
        {
            return PostAsync<TEntity>(_baseEndpoint, model);
        }

        public virtual async Task<ApiResponse<object>> UpdateAsync(int id, TUpdateModel model)
        {
            var endpoint = $"{_baseEndpoint}/{id}";
            var response = await PutAsync<object>(endpoint, model);

            if (response.IsSuccess && response.Data == null)
            {
                return new ApiResponse<object>
                {
                    IsSuccess = true,
                    Data = true,
                    Message = "Operación completada exitosamente"
                };
            }

            return response;
        }

        public virtual Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var endpoint = $"{_baseEndpoint}/{id}";
            return DeleteAsync(endpoint);
        }
    }
}