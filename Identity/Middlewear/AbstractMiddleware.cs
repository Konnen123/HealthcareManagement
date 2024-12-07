using Identity.Utils.MiddlewearUtils;
using Microsoft.AspNetCore.Http;

namespace Identity.Middlewear
{
    public abstract class AbstractMiddleware
    {
        public abstract Task InvokeAsync(HttpContext context);

        protected async Task WriteResponseMessage(HttpContext context, MiddlewearEnum error, string suggestion)
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

