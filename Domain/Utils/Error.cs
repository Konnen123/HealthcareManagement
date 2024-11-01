namespace Domain.Utils;

public sealed record Error(string Code, string? Description = null)
{
    public static readonly Error None = new(string.Empty);
    public override string ToString()
    {
        return $"Error {Code} occured. Details: {Description}";
    }
}