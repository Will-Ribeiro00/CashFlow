using CashFlow.Application.UseCases.Users.Delete;
using CashFlow.Domain.Entities;
using CummonTestUtilities.Entities;
using CummonTestUtilities.Repositories;
using CummonTestUtilities.Services;
using Shouldly;

namespace UseCase.Test.Users.Delete
{
    public class DeleteUserAccountUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var user = UserBuilder.Build();
            var useCase = CreateUseCase(user);

            //Act
            var act = async () => await useCase.Execute();

            //Assert
            await act.ShouldNotThrowAsync();
        }

        private DeleteUserAccountUseCase CreateUseCase(User user)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var repository = UserWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new DeleteUserAccountUseCase(loggedUser, repository, unitOfWork);
        }
    }
}
