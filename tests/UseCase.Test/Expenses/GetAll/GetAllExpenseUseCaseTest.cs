using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Domain.Entities;
using CummonTestUtilities.Entities;
using CummonTestUtilities.Mapper;
using CummonTestUtilities.Repositories;
using CummonTestUtilities.Services;
using Shouldly;

namespace UseCase.Test.Expenses.GetAll
{
    public class GetAllExpenseUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();
            var expenses = ExpenseBuilder.Collection(loggedUser);
            var useCase = CreateUseCase(loggedUser, expenses);

            //Act
            var result = await useCase.Execute();

            //Assign
            result.ShouldNotBeNull();
            //CONTINUAR
            foreach (var expense in result.Expenses)
            {
                expense.ShouldSatisfyAllConditions(
                    () => expense.ShouldNotBeNull(),
                    () => expense.Id.ShouldBeGreaterThan(0),
                    () => expense.Title.ShouldNotBeNullOrEmpty(),
                    () => expense.Amount.ShouldBeGreaterThan(0)
                    );
            }
        }
        private GetAllExpenseUseCase CreateUseCase(User user, List<Expense> expenses)
        {
            var repository = new ExpensesReadOnlyRepositoryBuilder().GetAll(user, expenses).Build();
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GetAllExpenseUseCase(repository, mapper, loggedUser);
        }
    }
}
