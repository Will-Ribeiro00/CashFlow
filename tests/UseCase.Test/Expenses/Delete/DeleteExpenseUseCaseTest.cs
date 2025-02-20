using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CummonTestUtilities.Entities;
using CummonTestUtilities.Repositories;
using CummonTestUtilities.Services;
using Shouldly;

namespace UseCase.Test.Expenses.Delete
{
    public class DeleteExpenseUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();
            var expense = ExpenseBuilder.Build(loggedUser);
            var useCase = CreateUseCase(loggedUser, expense);

            //Act
            var act = async () => await useCase.Execute(expense.Id);

            //Assert
            await act.ShouldNotThrowAsync();
        }

        [Fact]
        public async Task ErrorExpenseNotFound()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();
            var expense = ExpenseBuilder.Build(loggedUser);
            var useCase = CreateUseCase(loggedUser);

            //Act
            var act = async () => await useCase.Execute(id: 1000000);
            var result = await act.ShouldThrowAsync<NotFoundException>();

            //Assert
            result.GetErrors().Count().ShouldBe(1);
            result.GetErrors().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }

        private DeleteExpenseUseCase CreateUseCase(User user, Expense? expense = null)
        {
            var repositoryWriteOnly = ExpensesWriteOnlyRepositoryBuilder.Build();
            var repositoryReadOnly = new ExpensesReadOnlyRepositoryBuilder().GetById(expense, user).Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new DeleteExpenseUseCase(repositoryWriteOnly, repositoryReadOnly, loggedUser, unitOfWork);
        }
    }
}
