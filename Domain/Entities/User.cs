namespace Domain.Entities
{
    public abstract class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public DateOnly CreatedAt { get; set; }

        public bool IsEnabled { get; set; }
    }
}
