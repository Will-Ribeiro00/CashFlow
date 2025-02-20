using CashFlow.Communication.Responses.ResponseUserJson;

namespace CashFlow.Application.UseCases.Users.GetProfile
{
    public interface IGetUserProfileUseCase
    {
        Task<ResponseUserProfileJson> Execute();
    }
}
