
using Application.Use_Cases.Responses;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands.AuthCommands;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<Result<TokenResponse>>;