using CashFlow.Communication.Requests.RequestUserJson;

namespace CashFlow.Application.UseCases.Users.ChangePassword
{
    public interface IChangePasswordUseCase
    {
        Task Execute(RequestChangePasswordJson request);
    }
}
