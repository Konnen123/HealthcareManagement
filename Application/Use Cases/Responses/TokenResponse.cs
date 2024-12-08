namespace Application.Use_Cases.Responses;

public sealed class TokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    public TokenResponse(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}