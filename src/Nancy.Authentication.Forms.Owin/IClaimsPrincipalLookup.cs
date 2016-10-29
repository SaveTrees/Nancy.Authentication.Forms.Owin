namespace Nancy.Authentication.Forms.Owin
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    /// <summary>
    /// Extracts <see cref="ClaimsPrincipal"/> information for a user.
    /// </summary>
    public interface IClaimsPrincipalLookup
    {
        /// <summary>
        /// Determines whether a user exists, based on the <paramref name="userMapperId"/>.
        /// </summary>
        /// <param name="userMapperId">The Nancy user mapper id.</param>
        /// <param name="claimsPrincipal">The claims principle for the user, if found.</param>
        /// <returns><c>True</c> if a user can be found with the <paramref name="userMapperId"/>, <c>false</c> otherwise.</returns>
        Task<bool> UserExists(Guid userMapperId, out ClaimsPrincipal claimsPrincipal);

        /// <summary>
        ///
        /// </summary>
        ClaimsPrincipal GetAnonymousUserClaimsPrincipal();
    }
}