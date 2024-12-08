using Domain.Errors;
using Identity.Utils.MiddlewareUtils;
using Microsoft.AspNetCore.Http;

namespace Identity.Middleware
{
    public class ResponseMiddleware : AbstractMiddleware
    {
        private readonly RequestDelegate _next;
        public ResponseMiddleware(RequestDelegate next)
        {
            _next = next;   
        }

        public async override Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status403Forbidden && 
                context.Request.Path.StartsWithSegments("/api/v1/Auth/Login"))
            {              
                 
                await WriteResponseMessage(context, MiddlewareStatuses.FORBIDDEN, 
                    AuthErrors.UserAccountLocked("UserAuthentication",
                    "Account temporarely locked due to too many failed login attempts").ToString());
            }
            else if(context.Response.StatusCode == StatusCodes.Status403Forbidden &&
                !context.Request.Path.StartsWithSegments("/api/v1/Auth/Login"))
            {
                await WriteResponseMessage(context, MiddlewareStatuses.FORBIDDEN,
                    AuthorizationErrors.Forbidden("UserAuthentication",
                    "Ensure you have the rights to access this resource").ToString());
            }
            else if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                await WriteResponseMessage(context, MiddlewareStatuses.UNAUTHORIZED, 
                    AuthorizationErrors.Unauthorized("UserAuthentication","Unauthorized access for requested resource.").ToString());
            }
        }
    }

}
