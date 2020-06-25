using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MyExpenses.Services
{
    public interface IValidateHelper
    {
        /// <summary>
        /// Get user id from token/claims
        /// </summary>
        /// <param name="identity">Claims identity</param>
        /// <returns></returns>
        string GetUserId(ClaimsIdentity identity);
    }

    public class ValidateHelper : IValidateHelper
    {
        /// <inheritdoc>
        public string GetUserId(ClaimsIdentity identity)
        {
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