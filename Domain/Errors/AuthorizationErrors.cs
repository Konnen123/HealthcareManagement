using Domain.Utils;
namespace Domain.Errors
{
    public class AuthorizationErrors
    {
        public static Error Forbidden(string entityName, string description) => new Error($"{entityName}.Forbidden", description);
        public static Error Unauthorized(string entityName, string description) => new Error($"{entityName}.Unauthorized", description);
    }
}
