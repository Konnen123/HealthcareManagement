﻿using Domain.Entities.User;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using Identity.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Identity.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UsersDbContext _context;

        public UsersRepository(UsersDbContext context)
        {
            _context = context;
        }

        public async Task<Result<string>> Login(UserAuthentication user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<UserAuthentication>> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _context.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
                return account == null ? Result<UserAuthentication>.Failure(AuthErrors.EmailNotFound(nameof(UserAuthentication), email)) : Result<UserAuthentication>.Success(account);
            }
            catch (Exception e)
            {
                return Result<UserAuthentication>.Failure(EntityErrors.GetFailed(nameof(UserAuthentication), e.InnerException?.Message ?? $"An unexpected error occurred while retrieving the account with email {email}"));
            }
        }

        public async Task<Result<Unit>> UpdateUserPasswordAsync(UserAuthentication user, CancellationToken cancellationToken)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<Unit>.Success(Unit.Value);
            }
            catch (Exception ex)
            {
                return Result<Unit>.Failure(
                    EntityErrors.UpdateFailed(nameof(UserAuthentication),
                        ex.InnerException?.Message ?? "An unexpected error occurred while updating the user password")
                );
            }

        }

        public async Task<Result<UserAuthentication>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id, cancellationToken);
                return account == null ? Result<UserAuthentication>.Failure(EntityErrors.NotFound(nameof(UserAuthentication), id)) : Result<UserAuthentication>.Success(account);
            }
            catch (Exception e)
            {
                return Result<UserAuthentication>.Failure(EntityErrors.GetFailed(nameof(UserAuthentication), e.InnerException?.Message ?? $"An unexpected error occurred while retrieving the account with id {id}"));
            }
        }

        public async Task<Result<Guid>> Register(UserAuthentication user, CancellationToken cancellationToken)
        {
            Console.WriteLine(user.Password.Length);
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<Guid>.Success(user.UserId);
            }
            catch (Exception e)
            {
                return Result<Guid>.Failure(EntityErrors.CreateFailed(nameof(UserAuthentication),e.InnerException?.Message ?? "An unexpected error occurred while creating the user account"));
            }
        }
    }
}