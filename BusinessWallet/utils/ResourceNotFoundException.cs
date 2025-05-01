using System;

namespace BusinessWallet.utils
{
    /// <summary>
    /// Wordt gegooid als een gevraagde entiteit niet bestaat.
    /// </summary>
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string message) : base(message) { }
    }
}
