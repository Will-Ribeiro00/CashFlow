using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses
{
    public interface IExpensesReadOnlyRepository
    {
        Task<List<Expense>> GetAll(User user);
        Task<Expense?> GetById(int id, User user);
        Task<List<Expense>> FilterByMonth(DateOnly date, User user);
    }
}
