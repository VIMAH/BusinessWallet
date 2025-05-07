using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BusinessWallet.DTOs;
using BusinessWallet.data;
using BusinessWallet.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessWallet.services
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IMemoryCache _memoryCache;

        public AuthService(DataContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
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

        public async Task<AuthResponseValidateDto> ValidateChallengeAsync(AuthRequestValidateDto dto)
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
                    Action = "Unknown",
                    CredentialKey = "Unknown",
                    Result = true,
                    Reason = null,
                    CreatedAt = DateTime.UtcNow
                };

                _context.AuthorizationLogs.Add(log);
                await _context.SaveChangesAsync();

                _memoryCache.Remove(dto.ChallengeId);

                return new AuthResponseValidateDto
                {
                    EmployeeId = matchedEmployee.Id,
                    RoleId = employeeRole.RoleId,
                    ChallengeId = dto.ChallengeId
                };
            }
            else
            {
                throw new InvalidOperationException("ChallengeId heeft onverwacht type.");
            }
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
