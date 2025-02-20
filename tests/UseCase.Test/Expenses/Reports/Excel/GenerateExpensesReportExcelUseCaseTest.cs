using CashFlow.Application.UseCases.Expenses.Reports.Excel;
using CashFlow.Domain.Entities;
using CummonTestUtilities.Entities;
using CummonTestUtilities.Repositories;
using CummonTestUtilities.Services;
using Shouldly;

namespace UseCase.Test.Expenses.Reports.Excel
{
    public class GenerateExpensesReportExcelUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();
            var expenses = ExpenseBuilder.Collection(loggedUser);
            var useCase = CreateUseCase(expenses, loggedUser);

            //Act
            var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));

            //Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldNotBeEmpty()
                );
        }

        [Fact]
        public async Task SuccessEmpty()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();
            var useCase = CreateUseCase([], loggedUser);

            //Act
            var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));

            //Assert
            result.ShouldBeEmpty();
        }
        
        private GenerateExpensesReportExcelUseCase CreateUseCase(List<Expense> expenses, User user)
        {
            var repository = new ExpensesReadOnlyRepositoryBuilder().FilterByMonth(expenses, user).Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GenerateExpensesReportExcelUseCase(repository, loggedUser);
        }
    }
}
