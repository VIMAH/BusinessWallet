namespace BusinessWallet.DTOs
{
    public class ChallengeValidationResponseDto
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = null!;
    }
}
