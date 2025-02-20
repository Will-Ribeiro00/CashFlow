using CashFlow.Application.UseCases.Users.ChangePassword;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CummonTestUtilities.Cryptography;
using CummonTestUtilities.Entities;
using CummonTestUtilities.Repositories;
using CummonTestUtilities.Requests;
using CummonTestUtilities.Services;
using Shouldly;

namespace UseCase.Test.Users.ChangePassword
{
    public class ChangePasswordUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var user = UserBuilder.Build();
            var request = RequestChangePasswordJsonBuilder.Build();
            var useCase = CreateUseCase(user, request.Password);

            //Act 
            var act = async () => await useCase.Execute(request);

            //Assert
            await act.ShouldNotThrowAsync();
        }

        [Fact]
        public async Task ErrorNewPasswordEmpty()
        {
            //Arrange
            var user = UserBuilder.Build();
            var request = RequestChangePasswordJsonBuilder.Build();
            request.NewPassword = string.Empty;
            var useCase = CreateUseCase(user, request.Password);

            //Act 
            var act = async () => await useCase.Execute(request);
            var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

            //Assert
            result.GetErrors().ShouldHaveSingleItem();
            result.GetErrors().ShouldContain(ResourceErrorMessages.INVALID_PASSWORD);
        }

        [Fact]
        public async Task ErrorCurrentPasswordDifferent()
        {
            //Arrange
            var user = UserBuilder.Build();
            var request = RequestChangePasswordJsonBuilder.Build();
            var useCase = CreateUseCase(user);

            //Act 
            var act = async () => await useCase.Execute(request);
            var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

            //Assert
            result.GetErrors().ShouldHaveSingleItem();
            result.GetErrors().ShouldContain(ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD);
        }

        private ChangePasswordUseCase CreateUseCase(User user, string? password = null)
        {
            var unitOfWork = UnitOfWorkBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);
            var updateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
            var passwordEncrypter = new PasswordEncrypterBuilder().Verify(password).Build();

           return new ChangePasswordUseCase(updateRepository, loggedUser, passwordEncrypter, unitOfWork);
        }
    }
}
