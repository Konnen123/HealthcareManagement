
using System.Text.Json.Serialization;
using Application.Use_Cases.Responses;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands.AuthCommands;

public sealed class RefreshTokenCommand : IRequest<Result<TokenResponse>>
{
    public required string RefreshToken { get; set; }
    [JsonIgnore]
    public string? DeviceInfo { get; set; }
    [JsonIgnore]
    public string? IpAddress { get; set; }
}