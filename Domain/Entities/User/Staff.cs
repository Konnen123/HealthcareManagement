namespace Domain.Entities.User
{
    public abstract class Staff : User
    {
        public required string MedicalRank { get; set; }
    }
}
