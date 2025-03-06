using CashFlow.Communication.Requests.RequestUserJson;
using FluentValidation;

namespace CashFlow.Application.UseCases.Users.ChangePassword
{
    public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.NewPassword).SetValidator(new PasswordValidator<RequestChangePasswordJson>());
        }
    }
}
