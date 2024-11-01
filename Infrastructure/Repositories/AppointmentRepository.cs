using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext context;

        public AppointmentRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Guid> AddAsync(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Appointment> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            throw new NotImplementedException();
        }
    }
}
