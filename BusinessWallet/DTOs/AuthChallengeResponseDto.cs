namespace BusinessWallet.DTOs
{
    /// <summary>
    /// Response-payload voor POST /auth/challenge.
    /// </summary>
    public class AuthChallengeResponseDto
    {
        /// <summary>
        /// Unieke sleutel om het challenge-verzoek later te kunnen valideren.
        /// </summary>
        public Guid ChallengeId { get; set; }

        /// <summary>
        /// De willekeurige challenge (nonce) die door de client
        /// met de private key ondertekend moet worden.
        /// </summary>
        public string Challenge { get; set; } = string.Empty;
    }
}
