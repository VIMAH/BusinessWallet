using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BusinessWallet.DTOs;
using BusinessWallet.models;
using BusinessWallet.models.Enums;
using BusinessWallet.repository;               // ↔ IAuthRepository
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BusinessWallet.services
{
    /// <summary>
    /// Verwerkt de hele Identificatie-, Authenticatie- en Autorisatie-flow.
    /// </summary>
    public sealed class AuthService : IAuthService
    {
        private readonly IAuthRepository _repo;     // losgekoppeld van EF-Core
        private readonly IMemoryCache _cache;
        private readonly ILogger<AuthService> _logger;

        private const string CachePrefix = "Challenge-";
        private static readonly TimeSpan ChallengeTtl = TimeSpan.FromMinutes(10);

        public AuthService(
            IAuthRepository repo,
            IMemoryCache cache,
            ILogger<AuthService> logger)
        {
            _repo = repo;
            _cache = cache;
            _logger = logger;
        }

        // ───────────────────────────────────────────────────────────────
        // /auth/challenge
        // ───────────────────────────────────────────────────────────────
        public Task<AuthChallengeResponseDto> CreateChallengeAsync(
            AuthChallengeRequestDto request)
        {
            // 1) URL ontleden
            var parsed = ParseUrl(request.Url);

            // 2) Challenge genereren
            var challengeId = Guid.NewGuid();
            var nonce = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

            // 3) Cachen
            _cache.Set(
                $"{CachePrefix}{challengeId}",
                parsed with { Challenge = nonce },
                ChallengeTtl);

            _logger.LogInformation("Challenge aangemaakt voor {RequestedBy} – {Action}",
                                   parsed.RequestedBy, parsed.Action);

            // 4) DTO-response
            var response = new AuthChallengeResponseDto
            {
                ChallengeId = challengeId,
                Challenge = nonce
            };
            return Task.FromResult(response);   // ⚠️ CS1998 opgelost: geen async/await nodig
        }

        // ───────────────────────────────────────────────────────────────
        // /auth/validate
        // ───────────────────────────────────────────────────────────────
        public async Task<AuthValidateResponseDto> ValidateAsync(
            AuthValidateRequestDto request)
        {
            // 1) Challenge ophalen
            if (!_cache.TryGetValue($"{CachePrefix}{request.ChallengeId}",
                                    out ParsedChallenge? cached))
            {
                _logger.LogWarning("ChallengeId {Id} niet gevonden of verlopen",
                                   request.ChallengeId);
                return new AuthValidateResponseDto
                {
                    IsAuthorized = false,
                    Message = "ChallengeId niet gevonden of verlopen."
                };
            }

            // 2) Identificatie + authenticatie
            var (employee, role) =
                await FindEmployeeBySignatureAsync(cached.Challenge, request.Signature);

            if (employee is null || role is null)
            {
                _logger.LogWarning("Signature ongeldig – Challenge {Id}", request.ChallengeId);
                return new AuthValidateResponseDto
                {
                    IsAuthorized = false,
                    Message = "Signature ongeldig of medewerker/rol onbekend."
                };
            }

            // 3) Autorisatie
            bool allowed = await _repo.HasAllowedPolicyAsync(
                               cached.Action, cached.CredentialKey, employee, role);

            // 4) Logging naar DB
            var log = new AuthorizationLog
            {
                EmployeeId = employee.Id,
                RoleId = role.Id,
                RequestedBy = cached.RequestedBy,
                Action = cached.Action.ToString(),
                CredentialKey = cached.CredentialKey,
                AttributesJson = cached.AttributesJson,
                Result = allowed,
                Reason = allowed ? null : "Policy-weigering",
                CreatedAt = DateTime.UtcNow
            };
            await _repo.AddAuthorizationLogAsync(log);
            await _repo.SaveChangesAsync();

            // 5) Cache cleanup
            _cache.Remove($"{CachePrefix}{request.ChallengeId}");

            return new AuthValidateResponseDto
            {
                IsAuthorized = allowed,
                Message = allowed ? "U bent bevoegd" : "U bent niet bevoegd"
            };
        }

        // ───────────────────────────────────────────────────────────────
        // HELPERS
        // ───────────────────────────────────────────────────────────────

        private async Task<(Employee? employee, Role? role)> FindEmployeeBySignatureAsync(
            string challenge,
            string signatureB64)
        {
            foreach (var employee in await _repo.GetEmployeesWithRolesAsync())
            {
                if (string.IsNullOrWhiteSpace(employee.PublicKey)) continue;

                if (VerifySignature(challenge, signatureB64, employee.PublicKey))
                {
                    var role = employee.EmployeeRoles.FirstOrDefault()?.Role;
                    return (employee, role);
                }
            }
            return (null, null);
        }

        private static bool VerifySignature(
            string challenge,
            string signatureB64,
            string publicKeyPem)
        {
            try
            {
                using var rsa = RSA.Create();
                rsa.ImportFromPem(publicKeyPem);

                byte[] data = Encoding.UTF8.GetBytes(challenge);
                byte[] signature = Convert.FromBase64String(signatureB64);

                return rsa.VerifyData(
                    data, signature,
                    HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1);
            }
            catch
            {
                return false;
            }
        }

        private static ParsedChallenge ParseUrl(string url)
        {
            var uri = new Uri(url);
            var path = uri.AbsolutePath.ToLowerInvariant();

            ActionTypeEnum action;
            if (path.Contains("/issue")) action = ActionTypeEnum.Issue;
            else if (path.Contains("/verify")) action = ActionTypeEnum.Verify;
            else if (path.Contains("/revoke")) action = ActionTypeEnum.Revoke;
            else action = ActionTypeEnum.View;

            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            string credentialKey = query["cred"] ?? "Onbekend";
            string? attrsJson = query["attrs"];

            return new ParsedChallenge
            (
                Challenge: string.Empty,   // wordt later ingevuld
                Action: action,
                RequestedBy: uri.Host,
                CredentialKey: credentialKey,
                AttributesJson: attrsJson
            );
        }

        private sealed record ParsedChallenge(
            string Challenge,
            ActionTypeEnum Action,
            string RequestedBy,
            string CredentialKey,
            string? AttributesJson);
    }
}
