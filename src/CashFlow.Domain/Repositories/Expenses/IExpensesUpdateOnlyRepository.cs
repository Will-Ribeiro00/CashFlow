using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses
{
    public interface IExpensesUpdateOnlyRepository
    {
        Task<Expense?> GetById(int id);
        void Update(Expense expense);
    }
}
