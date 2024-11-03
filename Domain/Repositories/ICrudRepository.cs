using Domain.Utils;
using MediatR;

namespace Domain.Repositories;

public interface ICrudRepository<T>
{
    Task<Result<IEnumerable<T>>> GetAllAsync();
    Task<Result<T>> GetAsync(Guid id);
    Task<Result<Guid>> AddAsync(T _);
    Task<Result<Unit>> UpdateAsync(T _);
    Task<Result<Unit>> DeleteAsync(Guid id);
}