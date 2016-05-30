using Owin;

namespace Nancy.Authentication.Forms.Owin
{
    /// <summary>
    /// Owin extensions for <see cref="IAppBuilder"/>.
    /// </summary>
    public static class AppBuilderExtensions
    {
        /// <summary>
        /// Configures the builder to use the Nancy forms authentication.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="formsAuthenticationConfiguration">The forms authentication configuration.</param>
        /// <param name="claimsPrincipalLookup">The claims principle lookup.</param>
        /// <returns></returns>
        public static IAppBuilder UseNancyFormsAuthentication(
            this IAppBuilder builder,
            FormsAuthenticationConfiguration formsAuthenticationConfiguration,
            IClaimsPrincipalLookup claimsPrincipalLookup)
        {
            builder.Use(typeof(NancyFormsAuthenticationMiddleware), formsAuthenticationConfiguration, claimsPrincipalLookup);

            return builder;
        }
    }
}