using System.ComponentModel.DataAnnotations;

namespace BusinessWallet.DTOs
{
    /// <summary>
    /// Request-payload voor POST /auth/validate.
    /// </summary>
    public class AuthValidateRequestDto
    {
        /// <summary>
        /// Het eerder verkregen ChallengeId.
        /// </summary>
        [Required]
        public Guid ChallengeId { get; set; }

        /// <summary>
        /// De handtekening: sign(privateKey, challenge).
        /// </summary>
        [Required]
        public string Signature { get; set; } = string.Empty;
    }
}
