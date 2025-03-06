using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.Data_Acess.Repositories
{
    internal class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
    {
        private readonly CashFlowDbContext _context;
        public UserRepository(CashFlowDbContext context) => _context = context;

        public async Task Add(User user) => await _context.Users.AddAsync(user);

        public async Task<bool> ExistActiveUserWithEmail(string email) => await _context.Users.AnyAsync(user => user.Email.Equals(email));

        public async Task<User> GetById(int id) => await _context.Users.FirstAsync(user => user.Id == id);

        public async Task<User?> GetUserByEmail(string email) => await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));

        public void Update(User user) => _context.Users.Update(user);
        public async Task Delete(User user)
        {
            var userToRemove = await GetById(user.Id);

            _context.Users.Remove(userToRemove!);
        }
    }
}
