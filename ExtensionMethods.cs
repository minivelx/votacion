using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace WebApiVotacion
{
    public static class ExtensionMethods
    {
        /// Implementacion By Miguel Uribe
        /// Todos los resultados son extraidos de la decodificacion del token y no directamente de la base de datos

        public static string getUserId(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return null;

            ClaimsPrincipal currentUser = user;
            return currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static string getExpirationToken(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return null;

            ClaimsPrincipal principal = user;

            var Expiration = principal.Identities.FirstOrDefault().Claims.Where(x => x.Type == "exp").FirstOrDefault();
            return Expiration?.Value;
        }

        public static string getName(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return null;

            ClaimsPrincipal currentUser = user;
            return currentUser.FindFirst(ClaimTypes.GivenName)?.Value;
        }

        public static IList<string> getRoles(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return null;

            ClaimsPrincipal currentUser = user;
            List<string> roles = new List<string>();
            currentUser.FindAll(ClaimTypes.Role).ToList().ForEach(x => roles.Add(x.Value));

            return roles;
        }

    }
}