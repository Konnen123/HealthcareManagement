using Domain.Utils;

namespace Domain.Errors;

public static class EntityErrors
{
    public static Error CreateFailed(string entityName, string description) => new Error($"{entityName}.CreateFailed", description);
    public static Error NotFound(string entityName, Guid guid) => new Error($"{entityName}.NotFound", $"The {entityName} with id {guid} was not found");
    public static Error GetFailed(string entityName, string description) => new Error($"{entityName}.GetFailed", description);
    public static Error DeleteFailed(string entityName, string description) => new Error($"{entityName}.DeleteFailed", description);
    public static Error UpdateFailed(string entityName, string description) => new Error($"{entityName}.UpdateFailed", description);
}