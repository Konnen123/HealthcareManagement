﻿using Domain.Entities.User;
using Domain.Utils;
using MediatR;

namespace Domain.Repositories
{
    public interface IUsersRepository
    {
        Task<Result<Guid>> Register(UserAuthentication user, CancellationToken cancellationToken);
        Task<Result<string>> Login(UserAuthentication user, CancellationToken cancellationToken);
        Task<Result<UserAuthentication>> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<Result<UserAuthentication>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<Unit>> UpdateUserPasswordAsync(UserAuthentication user, CancellationToken cancellationToken);
    }
}