namespace CashFlow.Application.UseCases.Expenses.Delete
{
    public interface IDeleteExpenseUseCase
    {
        Task Execute(int id);
    }
}
