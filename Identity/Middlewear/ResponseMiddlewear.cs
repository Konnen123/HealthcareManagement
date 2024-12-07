using Identity.Utils.MiddlewearUtils;
using Microsoft.AspNetCore.Http;

namespace Identity.Middlewear
{
    public class ResponseMiddlewear : AbstractMiddleware
    {
        private readonly RequestDelegate _next;
        public ResponseMiddlewear(RequestDelegate next)
        {
            _next = next;   
        }

        public async override Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                await WriteResponseMessage(context, MiddlewearEnum.FORBIDDEN, 
                    "Ensure you have the correct role to access this resource.");
            }
            else if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                await WriteResponseMessage(context, MiddlewearEnum.UNAUTHORIZED, 
                    "Unauthorized access for requested resource.");
            }
        }
    }

}
