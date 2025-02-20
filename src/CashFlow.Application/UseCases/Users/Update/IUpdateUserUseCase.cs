using CashFlow.Communication.Requests.RequestUserJson;

namespace CashFlow.Application.UseCases.Users.Update
{
    public interface IUpdateUserUseCase
    {
        Task Execute(RequestUpdateUserJson request);
    }
}
