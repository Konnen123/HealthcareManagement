using Identity.Utils.MiddlewareUtils;
using Microsoft.AspNetCore.Http;

namespace Identity.Middleware
{
    public abstract class AbstractMiddleware
    {
        public abstract Task InvokeAsync(HttpContext context);

        protected async Task WriteResponseMessage(HttpContext context, MiddlewareStatuses error, string suggestion)
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

