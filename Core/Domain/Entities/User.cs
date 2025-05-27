using InternPulse4.Core.Domain.Enums;
using System.Text.Json.Serialization;

namespace InternPulse4.Core.Domain.Entities
{
    public class User : Auditables
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public Role Role { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public bool RememberMe { get; set; }
    }

}
