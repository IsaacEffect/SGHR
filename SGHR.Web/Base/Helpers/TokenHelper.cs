using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SGHR.Web.Base.Helpers
{
    public static class TokenHelper
    {
        public static int ObtenerIdClienteDesdeToken(HttpContext httpContext)
        {
            var token = httpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token)) return 0;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
            var claim = jwtToken?.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier || c.Type.EndsWith("/nameidentifier"));

            return claim != null ? int.Parse(claim.Value) : 0;
        }
    }

}
