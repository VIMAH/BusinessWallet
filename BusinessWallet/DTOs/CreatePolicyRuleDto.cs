using BusinessWallet.models.Enums;

namespace BusinessWallet.DTOs
{
    public class CreatePolicyRuleDto
    {
        /// <example>3</example>
        public ActionTypeEnum Action { get; set; }

        /// <example>Credential</example>
        public string TargetType { get; set; }

        /// <example>*</example>
        public string TargetValue { get; set; }

        /// <example>{ "Role": "Communicatie" }</example>
        public string ConditionJson { get; set; }

        /// <example>true</example>
        public bool IsAllowed { get; set; }

        /// <example>Communicatie toch</example>
        public string Message { get; set; }

        /// <example>2025-05-09T19:57:11.515Z</example>
        public DateTime? UpdatedAt { get; set; }
    }
}
