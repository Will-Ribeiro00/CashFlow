using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CummonTestUtilities.Entities;
using CummonTestUtilities.Mapper;
using CummonTestUtilities.Repositories;
using CummonTestUtilities.Requests;
using CummonTestUtilities.Services;
using Shouldly;

namespace UseCase.Test.Expenses.Register
{
    public class RegisterExpenseUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();
            var request = RequestExpenseJsonBuilder.Build();
            var useCase = CreateUseCase(loggedUser);

            //Act
            var result = await useCase.Execute(request);

            //Assert
            result.ShouldNotBeNull();
            result.Title.ShouldBe(request.Title);
        }

        [Fact]
        public async Task ErrorTitleEmpty()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();
            var request = RequestExpenseJsonBuilder.Build();
            request.Title = string.Empty;
            var useCase = CreateUseCase(loggedUser);

            //Act
            var act = async () => await useCase.Execute(request);
            var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

            //Assert
            result.GetErrors().Count.ShouldBe(1);
            result.GetErrors().ShouldContain(ResourceErrorMessages.TITLE_REQUIRED);
        }

        private RegisterExpenseUseCase CreateUseCase(User user)
        {
            var repository = ExpensesWriteOnlyRepositoryBuilder.Build();
            var mapper = MapperBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new RegisterExpenseUseCase(repository, unitOfWork, mapper, loggedUser);
        }
    }
}
