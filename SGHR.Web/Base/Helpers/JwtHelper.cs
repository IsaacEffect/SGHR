using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SGHR.Web.Base.Helpers
{
    public static class JwtHelper
    {
        public static int ObtenerIdDesdeToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return 0;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            var idClaim = jwtToken?.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier || c.Type.EndsWith("/nameidentifier")
            );

            return idClaim != null ? int.Parse(idClaim.Value) : 0;
        }
    }
}
