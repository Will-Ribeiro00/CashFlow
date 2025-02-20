using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CummonTestUtilities.Repositories
{
    public class ExpensesUpdateOnlyRepositoryBuilder
    {
        private readonly Mock<IExpensesUpdateOnlyRepository> _repository;

        public ExpensesUpdateOnlyRepositoryBuilder() => _repository = new Mock<IExpensesUpdateOnlyRepository>();

        public ExpensesUpdateOnlyRepositoryBuilder GetById(Expense? expense, User user)
        {
            if (expense is not null)
                _repository.Setup(repository => repository.GetById(expense.Id, user)).ReturnsAsync(expense);

            return this;
        }

        public IExpensesUpdateOnlyRepository Build() => _repository.Object;
    }
}
