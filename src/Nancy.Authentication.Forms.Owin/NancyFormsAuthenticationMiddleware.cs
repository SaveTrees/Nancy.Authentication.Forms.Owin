﻿namespace Nancy.Authentication.Forms.Owin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Cookies;

    /// <summary>
    /// The OWIN middleware for the nancy forms authentication.
    /// </summary>
    public class NancyFormsAuthenticationMiddleware
    {
        private const string ServerUser = "server.User";
        private const string CookieHeaderName = "Cookie";
        private readonly IClaimsPrincipalLookup _claimsPrincipalLookup;
        private readonly FormsAuthenticationConfiguration _formsAuthenticationConfiguration;
        private readonly Func<IDictionary<string, object>, Task> _next;

        public NancyFormsAuthenticationMiddleware(Func<IDictionary<string, object>, Task> next,
            FormsAuthenticationConfiguration formsAuthenticationConfiguration,
            IClaimsPrincipalLookup claimsPrincipalLookup)
        {
            _next = next;
            _formsAuthenticationConfiguration = formsAuthenticationConfiguration;
            _claimsPrincipalLookup = claimsPrincipalLookup;
        }

        /// <summary>
        /// Standard OWIN invoke.
        /// </summary>
        /// <param name="environment">The current environment.</param>
        /// <returns>The next invokable in the chain.</returns>
        public async Task Invoke(IDictionary<string, object> environment)
        {
            ClaimsPrincipal claimsPrincipal = null;
            var requestHeaders = (IDictionary<string, string[]>) environment["owin.RequestHeaders"];
            if (requestHeaders.ContainsKey(CookieHeaderName))
            {
                var authCookie = GetFormsAuthenticationCookies(requestHeaders[CookieHeaderName]).SingleOrDefault();
                if (authCookie != null)
                {
                    var user = FormsAuthentication.DecryptAndValidateAuthenticationCookie(authCookie.Value, _formsAuthenticationConfiguration);

                    Guid userMapperId;
                    if (Guid.TryParse(user, out userMapperId))
                    {
                        // The claimPrinciple is for the Anonymous user, if the user doesn't exist.
                        await _claimsPrincipalLookup.UserExists(userMapperId, out claimsPrincipal);
                    }
                }
            }

            if (claimsPrincipal == null)
            {
                claimsPrincipal = _claimsPrincipalLookup.GetAnonymousUserClaimsPrincipal();
            }

            if (environment.ContainsKey(ServerUser))
            {
                environment[ServerUser] = claimsPrincipal;
            }
            else
            {
                environment.Add(ServerUser, claimsPrincipal);
            }

            if (_next != null)
            {
                await _next.Invoke(environment);
            }
        }

        /// <summary>
        /// Extracts the forms authentication cookies from the request header.
        /// </summary>
        /// <param name="cookieHeaders"></param>
        /// <returns></returns>
        private static IEnumerable<NancyCookie> GetFormsAuthenticationCookies(IEnumerable<string> cookieHeaders)
        {
            return cookieHeaders
                .Select(h => h.Split(';'))
                .Select(header =>
                    header.Select(c =>
                    {
                        var pair = c.Split('=');
                        return new NancyCookie(pair[0].Trim(), pair[1]);
                    })
                    .SingleOrDefault(c => c.Name == FormsAuthentication.FormsAuthenticationCookieName));
        }
    }
}