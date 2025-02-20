using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Users
{
    public interface IUserUpdateOnlyRepository
    {
        Task<User> GetById(int id);
        void Update(User user);
    }
}
