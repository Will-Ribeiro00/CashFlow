using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CummonTestUtilities.Entities;
using CummonTestUtilities.Repositories;
using CummonTestUtilities.Requests;
using CummonTestUtilities.Services;
using Shouldly;

namespace UseCase.Test.Users.Update
{
    public class UpdateUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var user = UserBuilder.Build();
            var request = RequestUpdateUserJsonBuilder.Build();
            var useCase = CreateUseCase(user);

            //Act
            var act = async () => await useCase.Execute(request);
            await act.ShouldNotThrowAsync();

            //Assert
            user.Name.ShouldBe(request.Name);
            user.Email.ShouldBe(request.Email);
        }

        [Fact]
        public async Task ErrorNameEmpty()
        {
            //Arrange
            var user = UserBuilder.Build();
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;
            var useCase = CreateUseCase(user);

            //Act
            var act = async () => await useCase.Execute(request);
            var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

            //Assert
            result.ShouldSatisfyAllConditions(
                () => result.GetErrors().Count().ShouldBe(1),
                () => result.GetErrors().ShouldContain(ResourceErrorMessages.NAME_EMPTY)
                );
        }

        [Fact]
        public async Task ErrorEmailAlreadyExist()
        {
            //Arrange
            var user = UserBuilder.Build();
            var request = RequestUpdateUserJsonBuilder.Build();
            var useCase = CreateUseCase(user, request.Email);

            //Act
            var act = async () => await useCase.Execute(request);
            var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

            //Assert
            result.ShouldSatisfyAllConditions(
                () => result.GetErrors().Count().ShouldBe(1),
                () => result.GetErrors().ShouldContain(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED)
                );
        }

        private UpdateUserUseCase CreateUseCase(User user, string? email = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var updateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
            var readRepository = new UserReadOnlyRepositoryBuilder();
            var unitOfWork = UnitOfWorkBuilder.Build();

            if (!string.IsNullOrWhiteSpace(email))
                readRepository.ExistActiveUserWithEmail(email);

            return new UpdateUserUseCase(loggedUser, updateRepository, readRepository.Build(), unitOfWork);
        }
    }
}
