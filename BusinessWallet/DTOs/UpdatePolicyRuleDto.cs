using BusinessWallet.models.Enums;

namespace BusinessWallet.DTOs
{
    public class UpdatePolicyRuleDto
    {
        public ActionTypeEnum? Action { get; set; }
        public string? TargetType { get; set; }
        public string? TargetValue { get; set; }
        public string? ConditionJson { get; set; }
        public bool? IsAllowed { get; set; }
        public string? Message { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
