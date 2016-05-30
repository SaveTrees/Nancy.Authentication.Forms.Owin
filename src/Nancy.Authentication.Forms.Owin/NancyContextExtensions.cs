using System.Collections.Generic;
using System.Security.Claims;
using Nancy.Owin;

namespace Nancy.Authentication.Forms.Owin
{
    /// <summary>
    /// Extensions to the <see cref="NancyContext"/>.
    /// </summary>
    public static class NancyContextExtensions
    {
        /// <summary>
        /// Gets the current <see cref="ClaimsPrincipal"/> for the current user.
        /// </summary>
        /// <param name="context">The Nancy context.</param>
        /// <returns>The claims principle for the current user, or <c>null</c> if one cannot be found.</returns>
        public static ClaimsPrincipal GetClaimsPrincipal(this NancyContext context)
        {
            var environment = context.Items[NancyMiddleware.RequestEnvironmentKey] as IDictionary<string, object>;
            if (environment == null || !environment.ContainsKey(NancyMiddleware.ServerUser))
            {
                return null;
            }

            return environment[NancyMiddleware.ServerUser] as ClaimsPrincipal;
        }
    }
}