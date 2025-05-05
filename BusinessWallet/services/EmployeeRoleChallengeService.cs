using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BusinessWallet.data;
using BusinessWallet.DTOs;
using BusinessWallet.models;
using BusinessWallet.repository;
using Microsoft.EntityFrameworkCore;

namespace BusinessWallet.services
{
    public class EmployeeRoleChallengeService : IEmployeeRoleChallengeService
    {
        private readonly IEmployeeRoleChallengeRepository _challengeRepository;
        private readonly DataContext _context;

        public EmployeeRoleChallengeService(
            IEmployeeRoleChallengeRepository challengeRepository,
            DataContext context)
        {
            _challengeRepository = challengeRepository;
            _context = context;
        }

        public async Task<ChallengeResponseDto> CreateChallengeAsync(ChallengeRequestDto dto)
        {
            var existing = await _challengeRepository.GetValidChallengeAsync(dto.EmployeeId, dto.RoleId);
            if (existing != null)
            {
                return new ChallengeResponseDto
                {
                    Challenge = existing.Challenge
                };
            }

            var newChallenge = new EmployeeRoleChallenge
            {
                Id = Guid.NewGuid(),
                EmployeeId = dto.EmployeeId,
                RoleId = dto.RoleId,
                Challenge = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false
            };

            await _challengeRepository.AddAsync(newChallenge);
            await _challengeRepository.SaveChangesAsync();

            return new ChallengeResponseDto
            {
                Challenge = newChallenge.Challenge
            };
        }

        public async Task<ChallengeValidationResponseDto> ValidateChallengeAsync(ChallengeValidationRequestDto dto)
        {
            var challenge = await _challengeRepository.GetValidChallengeAsync(dto.EmployeeId, dto.RoleId);

            if (challenge == null)
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
                    Message = "Publieke sleutel niet gevonden voor deze medewerker."
                };
            }

            bool isSignatureValid;
            try
            {
                isSignatureValid = VerifySignature(challenge.Challenge, dto.Signature, employee.PublicKey);
            }
            catch
            {
                isSignatureValid = false;
            }

            if (!isSignatureValid)
            {
                return new ChallengeValidationResponseDto
                {
                    IsValid = false,
                    Message = "Signature is ongeldig."
                };
            }

            var role = await _context.Roles.FindAsync(dto.RoleId);
            if (role == null)
            {
                return new ChallengeValidationResponseDto
                {
                    IsValid = false,
                    Message = "Rol niet gevonden."
                };
            }

            var hasPermission = CheckPermission(role, dto.Action);

            if (!hasPermission)
            {
                return new ChallengeValidationResponseDto
                {
                    IsValid = false,
                    Message = $"Je hebt geen rechten om deze actie uit te voeren: {dto.Action}."
                };
            }

            challenge.IsUsed = true;
            await _challengeRepository.SaveChangesAsync();

            return new ChallengeValidationResponseDto
            {
                IsValid = true,
                Message = "Authenticatie en permissie succesvol gevalideerd."
            };
        }

        private bool CheckPermission(Role role, string action)
        {
            var propertyName = $"Can{char.ToUpper(action[0])}{action.Substring(1)}";
            var prop = typeof(Role).GetProperty(propertyName);
            if (prop == null || prop.PropertyType != typeof(bool))
            {
                return false;
            }

            return (bool)prop.GetValue(role)!;
        }

        private bool VerifySignature(string challenge, string signatureBase64, string publicKeyPem)
        {
            using var rsa = RSA.Create();
            rsa.ImportFromPem(publicKeyPem);

            byte[] dataBytes = Encoding.UTF8.GetBytes(challenge);
            byte[] signatureBytes = Convert.FromBase64String(signatureBase64);

            return rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}
