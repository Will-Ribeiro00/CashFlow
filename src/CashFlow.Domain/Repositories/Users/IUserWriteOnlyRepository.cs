using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Users
{
    public interface IUserWriteOnlyRepository
    {
        Task Add(Entities.User user);
        Task Delete(User user);
    }
}
