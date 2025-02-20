using CashFlow.Communication.Requests.RequestUserJson;
using CashFlow.Communication.Responses.ResponseUserJson;

namespace CashFlow.Application.UseCases.Users.Register
{
    public interface IRegisterUserUseCase
    {
        Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);
    }
}
