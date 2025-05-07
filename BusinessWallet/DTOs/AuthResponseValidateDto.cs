using System;

namespace BusinessWallet.DTOs
{
    public class AuthResponseValidateDto
    {
        public Guid EmployeeId { get; set; }
        public Guid RoleId { get; set; }
        public Guid ChallengeId { get; set; }
    }
}
