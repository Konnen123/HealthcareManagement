using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Queries.UserQueries;

public class VerifyEmailQuery : IRequest<Result<string>>
{
    public required string Token { get; set; }
}