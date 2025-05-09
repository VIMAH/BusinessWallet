// File: utils/JwtUtils.cs
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Jose;

namespace BusinessWallet.utils
{
    public class JwtUtils
    {
        private readonly RSA _publicKey;
        private readonly RSA _privateKey;

        public JwtUtils(RSA publicKey, RSA privateKey)
        {
            _publicKey = publicKey;
            _privateKey = privateKey;
        }

        /// <summary>
        /// Genereert een encrypted JWT (JWE) met de opgegeven claims.
        /// </summary>
        public string GenerateEncryptedToken(Guid employeeId, Guid roleId, Guid challengeId)
        {
            var payload = new Dictionary<string, object>
            {
                { "employeeId", employeeId.ToString() },
                { "roleId", roleId.ToString() },
                { "challengeId", challengeId.ToString() },
                { "exp", DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds() }
            };

            return JWT.Encode(payload, _publicKey, JweAlgorithm.RSA_OAEP_256, JweEncryption.A256GCM);
        }

        /// <summary>
        /// Decodeert en valideert een encrypted JWT (JWE) en retourneert de claims.
        /// </summary>
        public IDictionary<string, object> DecodeEncryptedToken(string token)
        {
            return JWT.Decode<Dictionary<string, object>>(token, _privateKey, JweAlgorithm.RSA_OAEP_256, JweEncryption.A256GCM);
        }
    }
}
