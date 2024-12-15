using Domain.Utils;

namespace Application.DTOs.UserDto
{
    public abstract class UserDto
    {
        public Guid UserId { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }

        public required string PhoneNumber { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public DateOnly CreatedAt { get; set; }

        public Roles Role { get; set; }
    }
}
