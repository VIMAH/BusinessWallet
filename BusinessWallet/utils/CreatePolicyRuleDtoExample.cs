using Swashbuckle.AspNetCore.Filters;
using BusinessWallet.DTOs;

namespace BusinessWallet.utils
{
    public class CreatePolicyRuleDtoExample : IExamplesProvider<CreatePolicyRuleDto>
    {
        public CreatePolicyRuleDto GetExamples()
        {
            return new CreatePolicyRuleDto
            {
                Action = BusinessWallet.models.Enums.ActionTypeEnum.Verify,
                TargetType = "Credential",
                TargetValue = "*",
                ConditionJson = "{ \"Role\": \"Communicatie\" }",
                IsAllowed = true,
                Message = "Communicatie toch",
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}
