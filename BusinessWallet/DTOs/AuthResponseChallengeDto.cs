using System;

namespace BusinessWallet.DTOs
{
    public class AuthResponseChallengeDto
    {
        public string Challenge { get; set; } = string.Empty;
        public Guid ChallengeId { get; set; }
    }
}
