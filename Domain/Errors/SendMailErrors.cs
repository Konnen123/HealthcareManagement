using Domain.Utils;

namespace Domain.Errors;

public static class SendMailErrors
{
    public static Error EmailNotSent(string entityName, string message) => new Error($"{entityName}.SendFailed", message);
}