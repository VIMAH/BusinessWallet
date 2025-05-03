using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BusinessWallet.data;
using BusinessWallet.DTOs;
using BusinessWallet.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessWallet.services
{
    public class EmployeeRoleChallengeService : IEmployeeRoleChallengeService
    {
        private readonly DataContext _context;
        private readonly ILogger<EmployeeRoleChallengeService> _logger;

        public EmployeeRoleChallengeService(DataContext context, ILogger<EmployeeRoleChallengeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ChallengeResponseDto> CreateChallengeAsync(ChallengeRequestDto dto)
        {
            // Check of Employee + Role relatie bestaat
            var exists = await _context.EmployeeRoles.AnyAsync(er =>
                er.EmployeeId == dto.EmployeeId && er.RoleId == dto.RoleId);

            if (!exists)
            {
                throw new InvalidOperationException("De opgegeven medewerker en rol bestaan niet of zijn niet gekoppeld.");
            }

            // Genereer een sterke challenge (bijv. 256-bit)
            var challengeBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(challengeBytes);
            }
            var challenge = Convert.ToBase64String(challengeBytes);

            var challengeEntity = new EmployeeRoleChallenge
            {
                EmployeeId = dto.EmployeeId,
                RoleId = dto.RoleId,
                Challenge = challenge,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };

            _context.EmployeeRoleChallenges.Add(challengeEntity);
            await _context.SaveChangesAsync();

            return new ChallengeResponseDto { Challenge = challenge };
        }

        public async Task<ChallengeValidationResponseDto> ValidateChallengeAsync(ChallengeValidationRequestDto dto)
        {
            // Zoek de laatste niet-verlopen challenge
            var challengeEntity = await _context.EmployeeRoleChallenges
                .Where(c => c.EmployeeId == dto.EmployeeId &&
                            c.RoleId == dto.RoleId &&
                            !c.IsUsed &&
                            c.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync();

            if (challengeEntity == null)
            {
                return new ChallengeValidationResponseDto
                {
                    IsValid = false,
                    Message = "Geen geldige challenge gevonden of challenge is verlopen."
                };
            }

            var employee = await _context.Employees.FindAsync(dto.EmployeeId);
            if (employee == null || string.IsNullOrEmpty(employee.PublicKey))
            {
                return new ChallengeValidationResponseDto
                {
                    IsValid = false,
                    Message = "Geen geldige medewerker of ontbrekende public key."
                };
            }

            try
            {
                bool isValid = VerifySignature(challengeEntity.Challenge, dto.Signature, employee.PublicKey);

                if (isValid)
                {
                    // Markeer challenge als gebruikt (optioneel)
                    challengeEntity.IsUsed = true;
                    await _context.SaveChangesAsync();

                    return new ChallengeValidationResponseDto
                    {
                        IsValid = true,
                        Message = "Authenticatie succesvol."
                    };
                }
                else
                {
                    return new ChallengeValidationResponseDto
                    {
                        IsValid = false,
                        Message = "Signature validatie mislukt."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij signature validatie");
                return new ChallengeValidationResponseDto
                {
                    IsValid = false,
                    Message = "Fout tijdens validatie."
                };
            }
        }

        private bool VerifySignature(string challenge, string signatureBase64, string publicKeyPem)
        {
            var signature = Convert.FromBase64String(signatureBase64);
            var data = Encoding.UTF8.GetBytes(challenge);

            using var rsa = RSA.Create();
            rsa.ImportFromPem(publicKeyPem);

            return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}
