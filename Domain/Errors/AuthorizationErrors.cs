using Domain.Utils;
namespace Domain.Errors
{
    public static class AuthorizationErrors
    {
        public static Error Forbidden(string entityName, string description) => new Error($"{entityName}.Forbidden", description);
        public static Error Unauthorized(string entityName, string description) => new Error($"{entityName}.Unauthorized", description);
        public static Error UserAccountLocked(string entityName, string description) => new Error($"{entityName}.AccountLocked", description);
    }
}
