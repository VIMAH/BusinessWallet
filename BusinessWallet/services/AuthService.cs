// File: services/AuthService.cs
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BusinessWallet.DTOs;
using BusinessWallet.data;
using BusinessWallet.models;
using BusinessWallet.repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using BusinessWallet.utils;

namespace BusinessWallet.services
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly JwtUtils _jwtUtils;
        private readonly IPolicyRulesRepository _policyRulesRepository;

        public AuthService(DataContext context, IMemoryCache memoryCache, JwtUtils jwtUtils, IPolicyRulesRepository policyRulesRepository)
        {
            _context = context;
            _memoryCache = memoryCache;
            _jwtUtils = jwtUtils;
            _policyRulesRepository = policyRulesRepository;
        }

        public async Task<AuthResponseChallengeDto> CreateChallengeAsync(AuthRequestChallengeDto dto)
        {
            var challengeId = Guid.NewGuid();
            var challengeString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

            _memoryCache.Set(challengeId, (challengeString, dto.CallbackUrl), TimeSpan.FromMinutes(10));

            return await Task.FromResult(new AuthResponseChallengeDto
            {
                Challenge = challengeString,
                ChallengeId = challengeId
            });
        }

        public async Task<AuthResponseTokenDto> GenerateTokenAsync(AuthRequestTokenDto dto)
        {
            if (!_memoryCache.TryGetValue(dto.ChallengeId, out var cachedObj))
            {
                throw new InvalidOperationException("ChallengeId niet gevonden of verlopen.");
            }

            if (cachedObj is ValueTuple<string, string> cachedTuple)
            {
                var challenge = cachedTuple.Item1;
                var callbackUrl = cachedTuple.Item2;

                Employee? matchedEmployee = null;
                foreach (var employee in await _context.Employees.ToListAsync())
                {
                    if (string.IsNullOrEmpty(employee.PublicKey))
                        continue;

                    if (VerifySignature(challenge, dto.Signature, employee.PublicKey))
                    {
                        matchedEmployee = employee;
                        break;
                    }
                }

                if (matchedEmployee == null)
                {
                    throw new InvalidOperationException("Signature is ongeldig. Geen geldige medewerker gevonden.");
                }

                var employeeRole = await _context.EmployeeRoles.FirstOrDefaultAsync(er => er.EmployeeId == matchedEmployee.Id);
                if (employeeRole == null)
                {
                    throw new InvalidOperationException("Geen rol gevonden voor deze medewerker.");
                }

                var log = new AuthorizationLog
                {
                    EmployeeId = matchedEmployee.Id,
                    RoleId = employeeRole.RoleId,
                    RequestedBy = callbackUrl,
                    Action = "Authentication",
                    CredentialKey = "N/A",
                    Result = true,
                    Reason = null,
                    CreatedAt = DateTime.UtcNow
                };

                _context.AuthorizationLogs.Add(log);
                await _context.SaveChangesAsync();

                _memoryCache.Remove(dto.ChallengeId);

                // generate encrypted JWT token
                var token = _jwtUtils.GenerateEncryptedToken(matchedEmployee.Id, employeeRole.RoleId, dto.ChallengeId);

                return new AuthResponseTokenDto
                {
                    AccessToken = token
                };
            }
            else
            {
                throw new InvalidOperationException("ChallengeId heeft onverwacht type.");
            }
        }

        public async Task<AuthResponseCredentialsDto> GetCredentialsAsync(ClaimsPrincipal user)
        {
            var employeeIdClaim = user.FindFirst("employeeId")?.Value;
            var roleIdClaim = user.FindFirst("roleId")?.Value;
            var challengeIdClaim = user.FindFirst("challengeId")?.Value;

            if (employeeIdClaim == null || roleIdClaim == null)
            {
                throw new UnauthorizedAccessException("Token mist noodzakelijke claims.");
            }

            var employeeId = Guid.Parse(employeeIdClaim);
            var roleId = Guid.Parse(roleIdClaim);

            // query policy rules voor deze employee + role
            var allowedCredentials = await _policyRulesRepository.GetAllowedCredentialsAsync(employeeId, roleId);

            var response = new AuthResponseCredentialsDto();
            foreach (var cred in allowedCredentials)
            {
                response.Credentials.Add(new CredentialDto
                {
                    Claim = cred.TargetType,   // ← gebruiken van jouw model property
                    Value = cred.TargetValue
                });


            }

            return response;
        }

        private static bool VerifySignature(string challenge, string signatureBase64, string publicKeyPem)
        {
            try
            {
                using var rsa = RSA.Create();
                rsa.ImportFromPem(publicKeyPem);

                byte[] dataBytes = Encoding.UTF8.GetBytes(challenge);
                byte[] signatureBytes = Convert.FromBase64String(signatureBase64);

                return rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
            catch
            {
                return false;
            }
        }
    }
}
