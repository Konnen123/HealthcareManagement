﻿using Domain.Utils;

namespace Domain.Entities.User
{
    public abstract class User
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }

        public DateOnly CreatedAt { get; set; }

        public bool IsEnabled { get; set; }

        public RolesEnum Role { get; set; }
    }
}