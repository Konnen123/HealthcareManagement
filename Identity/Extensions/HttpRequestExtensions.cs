using Microsoft.AspNetCore.Http;

namespace Identity.Extensions;

public static class HttpRequestExtensions
{
    public static HttpRequest Clone(this HttpRequest request)
    {
        var newRequest = new DefaultHttpContext().Request;
        newRequest.Scheme = request.Scheme;
        newRequest.Host = request.Host;
        newRequest.Path = request.Path;
        newRequest.QueryString = request.QueryString;
        newRequest.Method = request.Method;

        foreach (var header in request.Headers)
        {
            newRequest.Headers[header.Key] = header.Value;
        }

        return newRequest;
    }
}