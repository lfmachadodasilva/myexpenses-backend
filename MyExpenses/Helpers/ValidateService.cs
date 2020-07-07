using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MyExpenses.Helpers
{
    public interface IValidateHelper
    {
        /// <summary>
        /// Get user id from token/claims
        /// </summary>
        /// <param name="httpContext">Context</param>
        /// <returns></returns>
        string GetUserId(HttpContext httpContext);
    }

    public class ValidateHelper : IValidateHelper
    {
        /// <inheritdoc>
        public string GetUserId(HttpContext httpContext)
        {
            var identity = httpContext?.User?.Identity as ClaimsIdentity;

            if (identity == null)
            {
                return null;
            }

            IEnumerable<Claim> claims = identity.Claims;
            var claim = claims.FirstOrDefault(x => x.Type.Equals("user_id"));
            if (claim == null)
            {
                return null;
            }

            return claim.Value;
        }
    }
}