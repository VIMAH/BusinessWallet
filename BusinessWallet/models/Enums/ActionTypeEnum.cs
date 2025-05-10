namespace BusinessWallet.models.Enums
{
    /// <summary>
    /// Alle hoofd­acties die in PolicyRules kunnen voorkomen.
    /// </summary>
    public enum ActionTypeEnum
    {
        /// <summary>Alleen lezen/bekijken.</summary>
        View = 0,

        /// <summary>Een nieuwe credential of resource uitgeven.</summary>
        Issue = 1,

        /// <summary>Een aangeleverde credential of handtekening verifiëren.</summary>
        Verify = 2,

        /// <summary>Een eerder uitgegeven credential of recht intrekken.</summary>
        Revoke = 3
    }
}
