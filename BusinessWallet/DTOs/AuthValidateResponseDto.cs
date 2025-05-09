namespace BusinessWallet.DTOs
{
    /// <summary>
    /// Response-payload voor POST /auth/validate.
    /// Geeft het resultaat van identificatie, authenticatie en autorisatie.
    /// </summary>
    public class AuthValidateResponseDto
    {
        /// <summary>
        /// True als de gebruiker voldoet aan de policies.
        /// </summary>
        public bool IsAuthorized { get; set; }

        /// <summary>
        /// Menselijke toelichting, bv. "U bent bevoegd" of "U bent niet bevoegd".
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
