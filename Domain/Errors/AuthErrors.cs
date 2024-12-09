using Domain.Utils;

namespace Domain.Errors;

public static class AuthErrors
{
    public static Error EmailNotFound(string entityName, string email) => new Error($"{entityName}.EmailNotFound", $"The {entityName} with email {email} was not found");
    public static Error LoginFailed(string entityName, string description) => new Error($"{entityName}.LoginFailed", description);
    public static Error UserAccountLocked(string entityName, string description) => new Error($"{entityName}.AccountLocked", description);
    public static Error UserNotLoggedIn(string entityName, string description) => new Error($"{entityName}.NotLoggedIn", description);
}