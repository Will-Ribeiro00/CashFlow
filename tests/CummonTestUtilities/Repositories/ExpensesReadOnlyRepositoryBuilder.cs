using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CummonTestUtilities.Repositories
{
    public class ExpensesReadOnlyRepositoryBuilder
    {
        private readonly Mock<IExpensesReadOnlyRepository> _repository;
        public ExpensesReadOnlyRepositoryBuilder()
        {
            _repository = new Mock<IExpensesReadOnlyRepository>();
        }

        public ExpensesReadOnlyRepositoryBuilder GetAll(User user, List<Expense> expenses)
        {
            _repository.Setup(repository => repository.GetAll(user)).ReturnsAsync(expenses);

            return this;
        }
        public ExpensesReadOnlyRepositoryBuilder GetById(Expense expenses, User user)
        {
            if (expenses is not null)
                _repository.Setup(repository => repository.GetById(expenses.Id, user)).ReturnsAsync(expenses);

            return this;
        }
        public ExpensesReadOnlyRepositoryBuilder FilterByMonth(List<Expense> expenses, User user)
        {
            _repository.Setup(repository => repository.FilterByMonth(It.IsAny<DateOnly>(), user)).ReturnsAsync(expenses);
            return this;
        }

        public IExpensesReadOnlyRepository Build() => _repository.Object;
    }
}
