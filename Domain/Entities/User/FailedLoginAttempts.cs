﻿namespace Domain.Entities.User
{
    public class FailedLoginAttempts
    {
        public class FailedLoginAttempt
        {
            public Guid AttemptId { get; set; }
            public Guid UserId { get; set; }
            public int FailedAttempts { get; set; }
            public DateTime LastFailedAttemptTime { get; set; }
            public DateTime? LockoutEndTime { get; set; }
            public int MaxFailedLoginAttempts { get; set; } = 5;

            public UserAuthentication UserAuthentication { get; set; } = null!;
        }
    }
}