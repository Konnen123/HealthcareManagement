using Identity.Utils.MiddlewearUtils;
using Microsoft.AspNetCore.Http;

namespace Identity.Middlewear
{
    public class ResponseMiddlewear
    {
        private readonly RequestDelegate _next;

        public ResponseMiddlewear(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                await WriteResponseMessage(context, AuthorizationEnum.ACCESS_DENIED, 
                    "Ensure you have the correct role to access this resource.");
            }
            else if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                await WriteResponseMessage(context, AuthorizationEnum.ACCESS_DENIED, 
                    "Unauthorized access for requested resource.");
            }
        }

        private static async Task WriteResponseMessage(HttpContext context, AuthorizationEnum error, string suggestion)
        {
            var message = new
            {
                Error = error.ToString(),
                Suggestion = suggestion
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(message);
        }

    }

}
