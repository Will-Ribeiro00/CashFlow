using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CummonTestUtilities.Repositories
{
    public class ExpensesWriteOnlyRepositoryBuilder
    {
        public static IExpensesWriteOnlyRepository Build() => new Mock<IExpensesWriteOnlyRepository>().Object;
    }
}
