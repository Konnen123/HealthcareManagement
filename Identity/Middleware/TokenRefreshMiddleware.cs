using System.Net.Http.Headers;
using System.Text;
using Domain.Repositories;
using Domain.Services;
using Identity.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Middleware;

public class TokenRefreshMiddleware : AbstractMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _serviceScopeFactory;  // Inject the IServiceScopeFactory

    public TokenRefreshMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public override async Task InvokeAsync(HttpContext httpContext)
{
    // Capture the original response status
    var originalStatusCode = httpContext.Response.StatusCode;

    // Create a wrapper to capture the response
    var originalBodyStream = httpContext.Response.Body;
    using var responseBody = new MemoryStream();
    httpContext.Response.Body = responseBody;

    try
    {
        await _next(httpContext);

        // Reset the stream position
        responseBody.Seek(0, SeekOrigin.Begin);

        // Check if we need to refresh token
        if (httpContext.Response.StatusCode == 401)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
                var usersRepository = scope.ServiceProvider.GetRequiredService<IUsersRepository>();

                var refreshToken = httpContext.Request.Headers["Refresh-Token"].ToString() ?? 
                    throw new Exception("Could not find refresh token in request");
                
                var refreshInstance = await tokenService.ValidateRefreshTokenAsync(refreshToken);
                if (refreshInstance is null)
                {
                    throw new Exception("No corresponding refresh token found");
                }
                
                var user = await usersRepository.GetByIdAsync(refreshInstance.UserId, new CancellationToken());
                if (!user.IsSuccess)
                {
                    throw new Exception("User not found");
                }
                
                var newAccessToken = tokenService.GenerateAccessToken(user.Value!);

                // Create a new HttpClient to re-execute the request
                using var httpClient = new HttpClient();
                
                // Prepare the new request
                var originalRequest = httpContext.Request;
                var newRequest = new HttpRequestMessage
                {
                    Method = new HttpMethod(originalRequest.Method),
                    RequestUri = new Uri($"{originalRequest.Scheme}://{originalRequest.Host}{originalRequest.Path}{originalRequest.QueryString}")
                };

                // Copy headers from original request
                foreach (var header in originalRequest.Headers)
                {
                    newRequest.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }

                // Update Authorization header
                newRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newAccessToken);

                // If there's a body, copy it
                if (originalRequest.ContentLength > 0)
                {
                    originalRequest.Body.Seek(0, SeekOrigin.Begin);
                    var bodyContent = await new StreamReader(originalRequest.Body).ReadToEndAsync();
                    newRequest.Content = new StringContent(bodyContent, Encoding.UTF8, originalRequest.ContentType);
                }

                // Send the new request
                var response = await httpClient.SendAsync(newRequest);

                // Copy the new response back to the original context
                httpContext.Response.StatusCode = (int)response.StatusCode;
                foreach (var header in response.Headers)
                {
                    httpContext.Response.Headers[header.Key] = header.Value.ToArray();
                }

                await response.Content.CopyToAsync(originalBodyStream);
            }
        }
        else
        {
            // If not a 401, copy the original response back
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
    finally
    {
        // Restore the original body stream
        httpContext.Response.Body = originalBodyStream;
    }
}
    
}
