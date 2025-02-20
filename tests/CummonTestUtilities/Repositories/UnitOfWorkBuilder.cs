using CashFlow.Domain.Repositories;
using Moq;

namespace CummonTestUtilities.Repositories
{
    public class UnitOfWorkBuilder
    {
        public static IUnitOfWork Build() =>  new Mock<IUnitOfWork>().Object;
    }
}
