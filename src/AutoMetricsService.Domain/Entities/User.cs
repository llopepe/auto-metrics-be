using Core.Framework.Domain.Common;

namespace AutoMetricsService.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // Puedes agregar Roles como string CSV o tener tabla many-to-many para roles.
        public string Roles { get; set; } = "User"; // ejemplo "Admin,User"
    }
}
