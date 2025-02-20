using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CummonTestUtilities.Entities;
using CummonTestUtilities.Mapper;
using CummonTestUtilities.Repositories;
using CummonTestUtilities.Requests;
using CummonTestUtilities.Services;
using Shouldly;

namespace UseCase.Test.Expenses.Update
{
    public class UpdateExpenseUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();
            var expense = ExpenseBuilder.Build(loggedUser);
            var request = RequestExpenseJsonBuilder.Build();
            var useCase = CreateUseCase(loggedUser, expense);

            //Act 
            var act = async () => await useCase.Execute(expense.Id, request);

            //Assert
            await act.ShouldNotThrowAsync();

            expense.Title.ShouldBe(request.Title);
            expense.Description.ShouldBe(request.Description);
            expense.Date.ShouldBe(request.Date);
            expense.Amount.ShouldBe(request.Amount);
            expense.PaymentType.ShouldBe((PaymentType)request.PaymentType);
        }

        [Fact]
        public async Task ErrorTitleEmpty()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();
            var expense = ExpenseBuilder.Build(loggedUser);

            var request = RequestExpenseJsonBuilder.Build();
            request.Title = string.Empty;

            var useCase = CreateUseCase(loggedUser, expense);
            var act = async () => await useCase.Execute(expense.Id, request);

            //Act
            var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

            //Assert
            result.GetErrors().Count().ShouldBe(1);
            result.GetErrors().Contains(ResourceErrorMessages.TITLE_REQUIRED);
        }

        [Fact]
        public async Task ErrorExpenseNotFound()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();
            var request = RequestExpenseJsonBuilder.Build();
            var useCase = CreateUseCase(loggedUser);
            var act = async () => await useCase.Execute(id: 100000000, request);
            
            //Act
            var result = await act.ShouldThrowAsync<NotFoundException>();

            //Assert
            result.GetErrors().Count().ShouldBe(1);
            result.GetErrors().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }

        private UpdateExpenseUseCase CreateUseCase(User user, Expense? expense = null)
        {
            var repository = new ExpensesUpdateOnlyRepositoryBuilder().GetById(expense, user).Build();
            var mapper = MapperBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new UpdateExpenseUseCase(mapper, unitOfWork, repository, loggedUser);
        }
    }
}
