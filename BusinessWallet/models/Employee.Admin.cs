namespace BusinessWallet.models
{
    public partial class Employee
    {
        /// <summary>
        /// True = super-user; negeert alle RBAC-checks.
        /// </summary>
        public bool IsAdmin { get; set; } = false;

        // Navigatie naar rollen
        public ICollection<EmployeeRole>? EmployeeRoles { get; set; }
    }
}
