using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CummonTestUtilities.Repositories
{
    public class UserWriteOnlyRepositoryBuilder
    {
        public static IUserWriteOnlyRepository Build() => new Mock<IUserWriteOnlyRepository>().Object;
    }
}
