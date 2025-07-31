using SGHR.Web.ViewModel;

namespace SGHR.Web.ApiServices.Interfaces
{
    public interface ICrudApiService<TEntity, TCreateModel, TUpdateModel>
    {
        Task<ApiResponse<List<TEntity>>> GetAllAsync();
        Task<ApiResponse<TUpdateModel>> GetByIdAsync(int id);
        Task<ApiResponse<TEntity>> CreateAsync(TCreateModel model);
        Task<ApiResponse<object>> UpdateAsync(int id, TUpdateModel model);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}
