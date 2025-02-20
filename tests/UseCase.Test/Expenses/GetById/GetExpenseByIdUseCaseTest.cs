using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CummonTestUtilities.Entities;
using CummonTestUtilities.Mapper;
using CummonTestUtilities.Repositories;
using CummonTestUtilities.Services;
using Shouldly;

namespace UseCase.Test.Expenses.GetById
{
    public class GetExpenseByIdUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();
            var expense = ExpenseBuilder.Build(loggedUser);
            var useCase = CreateUseCase(loggedUser, expense);

            //Act
            var result = await useCase.Execute(expense.Id);

            //Assert 
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Id.ShouldBe(expense.Id),
                () => result.Title.ShouldBe(expense.Title),
                () => result.Description.ShouldBe(expense.Description),
                () => result.Date.ShouldBe(expense.Date),
                () => result.Amount.ShouldBe(expense.Amount),
                () => result.PaymentType.ShouldBe((CashFlow.Communication.Enums.PaymentType)expense.PaymentType),
                () => result.Tags.ShouldNotBeNull(),
                () => result.Tags.Select(tag => tag.ToString()).ShouldBe(expense.Tags.Select(tag => tag.Value.ToString()))
                );
        }

        [Fact]
        public async Task ErrorExpenseNotFound()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();
            var useCase = CreateUseCase(loggedUser); ;

            //Act
            var act = async () => await useCase.Execute(id: 1000);
            var result = await act.ShouldThrowAsync<NotFoundException>();

            //Assert 
            result.GetErrors().Count().ShouldBe(1);
            result.GetErrors().ShouldContain(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }

        private GetExpenseByIdUseCase CreateUseCase(User user, Expense? expense = null)
        {
            var repository = new ExpensesReadOnlyRepositoryBuilder().GetById(expense, user).Build();
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GetExpenseByIdUseCase(repository, mapper, loggedUser);
        }
    }
}
