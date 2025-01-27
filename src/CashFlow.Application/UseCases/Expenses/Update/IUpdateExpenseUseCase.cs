using CashFlow.Communication.Requests.RequestExpenseJson;

namespace CashFlow.Application.UseCases.Expenses.Update
{
    public interface IUpdateExpenseUseCase
    {
        Task Execute(int id, RequestExpenseJson request);
    }
}
