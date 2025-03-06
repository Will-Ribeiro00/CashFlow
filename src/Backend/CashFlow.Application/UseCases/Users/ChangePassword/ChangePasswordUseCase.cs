using CashFlow.Communication.Requests.RequestUserJson;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.ChangePassword
{
    public class ChangePasswordUseCase : IChangePasswordUseCase
    {
        private readonly IUserUpdateOnlyRepository _updateRepository;
        private readonly ILoggedUser _loggedUser;
        private readonly IPasswordEncrypter _passwordEncrypter;
        private readonly IUnitOfWork _unitOfWork;

        public ChangePasswordUseCase(IUserUpdateOnlyRepository updateRepository, ILoggedUser loggedUser, IPasswordEncrypter passwordEncrypter, IUnitOfWork unitOfWork)
        {
            _updateRepository = updateRepository;
            _loggedUser = loggedUser;
            _passwordEncrypter = passwordEncrypter;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(RequestChangePasswordJson request)
        {
            var loggedUser = await _loggedUser.Get();

            Validate(request, loggedUser);

            var user = await _updateRepository.GetById(loggedUser.Id);
            user.Password = _passwordEncrypter.Encrypt(request.NewPassword);

            _updateRepository.Update(user);

            await _unitOfWork.Commit();
        }

        private void Validate(RequestChangePasswordJson request, User loggedUser)
        {
            var validator = new ChangePasswordValidator();

            var result = validator.Validate(request);

            var passwordMatch = _passwordEncrypter.Verify(request.Password, loggedUser.Password);

            if (!passwordMatch)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
            }

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
