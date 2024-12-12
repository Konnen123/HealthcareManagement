using System.ComponentModel.DataAnnotations;
using Domain.Utils;

namespace Domain.Entities.User
{
    public abstract class User
    {
        public Guid UserId { get; set; }
        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }
        public required string Password { get; set; }

        public required string PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }

        public DateOnly CreatedAt { get; set; }

        public bool IsEnabled { get; set; }

        public Roles Role { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
