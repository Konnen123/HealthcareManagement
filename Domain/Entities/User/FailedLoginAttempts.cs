namespace Domain.Entities.User
{
    public class FailedLoginAttempt
    {
        public Guid AttemptId { get; set; }
        public Guid UserId { get; set; }
        public int FailedAttempts { get; set; }
        public DateTime LastFailedAttemptTime { get; set; }
        public DateTime? LockoutEndTime { get; set; }
        public int MaxFailedLoginAttempts { get; set; } = 5;

<<<<<<< HEAD
            public User UserAuthentication { get; set; } = null!;
        }
=======
        public UserAuthentication UserAuthentication { get; set; } = null!;
>>>>>>> 6fc9b82 (Added repos for forgot and reset password,entities and token logic)
    }
}