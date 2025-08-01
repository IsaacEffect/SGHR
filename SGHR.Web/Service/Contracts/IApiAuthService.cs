using SGHR.Web.Models;

namespace SGHR.Web.Service.Contracts
{
    public interface IApiAuthService
    {
        Task<string?> AutenticarAsync(AuthModel model);
    }
}
