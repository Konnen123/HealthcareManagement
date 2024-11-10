namespace Domain.Entities
{
    public abstract class Staff : User
    {
        public required string MedicalRank { get; set; }
    }
}
