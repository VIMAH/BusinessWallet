// File: DTOs/AuthResponseCredentialsDto.cs
using System.Collections.Generic;

namespace BusinessWallet.DTOs
{
    public class AuthResponseCredentialsDto
    {
        public List<CredentialDto> Credentials { get; set; } = new List<CredentialDto>();
    }

    public class CredentialDto
    {
        public string Claim { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
