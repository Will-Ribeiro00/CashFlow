using CashFlow.Communication.Responses.ResponseExpenseJson;

namespace CashFlow.Application.UseCases.Expenses.GetById
{
    public interface IGetExpenseByIdUseCase
    {
        Task<ResponseExpenseJson> Execute(int id);
    }
}
