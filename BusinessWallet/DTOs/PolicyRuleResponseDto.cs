using BusinessWallet.models.Enums;

namespace BusinessWallet.DTOs
{
    public class PolicyRuleResponseDto
    {
        public Guid Id { get; set; }
        public ActionTypeEnum Action { get; set; }
        public string TargetType { get; set; } = string.Empty;
        public string TargetValue { get; set; } = string.Empty;
        public string ConditionJson { get; set; } = "{}";
        public bool IsAllowed { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
