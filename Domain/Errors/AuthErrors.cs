using Domain.Utils;

namespace Domain.Errors;

public static class AuthErrors
{
    public static Error EmailNotFound(string entityName, string email) => new Error($"{entityName}.EmailNotFound", $"The {entityName} with email {email} was not found");
    public static Error EmailAlreadyExists(string entityName, string email) => new Error($"{entityName}.EmailAlreadyExists", $"The {entityName} with email {email} already exists");
    public static Error LoginFailed(string entityName, string description) => new Error($"{entityName}.LoginFailed", description);
    public static Error UserAccountLocked(string entityName, string description) => new Error($"{entityName}.AccountLocked", description);
    
    public static Error InvalidResetPasswordToken(string entityName, string description) => new Error($"{entityName}.InvalidResetPasswordToken", description);
    
    public static Error ResetPasswordFailed(string entityName, string description) => new Error($"{entityName}.ResetPasswordFailed", description);
    public static Error UserNotLoggedIn(string entityName, string description) => new Error($"{entityName}.NotLoggedIn", description);
}