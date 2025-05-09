using System.ComponentModel.DataAnnotations;

namespace BusinessWallet.DTOs
{
    /// <summary>
    /// Request-payload voor POST /auth/challenge.
    /// </summary>
    public class AuthChallengeRequestDto
    {
        /// <summary>
        /// De URL waarvoor een challenge vereist is
        /// (bijv. een Issue-, Verify- of andere OpenID-/OIDC-achtige call).
        /// </summary>
        [Required]
        public string Url { get; set; } = string.Empty;
    }
}
