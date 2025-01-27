using CashFlow.Communication.Requests.RequestExpenseJson;
using CashFlow.Communication.Responses.ResponseExpenseJson;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public interface IRegisterExpenseUseCase
    {
        Task<ResponseRegisterExpenseJson> Execute(RequestExpenseJson request);
    }
}
