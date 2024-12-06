using Domain.Entities.User;
using Domain.Repositories;
using Domain.Utils;
using Microsoft.Extensions.Configuration;

namespace Identity.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        //private readonly UsersDbContext usersDbContext;
        private readonly IConfiguration configuration;

        public UsersRepository(IConfiguration configuration)
        {
           // this.usersDbContext = usersDbContext;
            this.configuration = configuration;
        }

        public async Task<Result<string>> Login(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Guid>> Register(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

