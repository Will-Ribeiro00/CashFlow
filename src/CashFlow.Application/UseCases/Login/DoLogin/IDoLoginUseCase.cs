using CashFlow.Communication.Requests.RequestUserJson;
using CashFlow.Communication.Responses.ResponseUserJson;

namespace CashFlow.Application.UseCases.Login.DoLogin
{
    public interface IDoLoginUseCase
    {
        Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
    }
}
