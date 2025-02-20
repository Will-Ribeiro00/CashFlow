using CashFlow.Application.UseCases.Login.DoLogin;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CummonTestUtilities.Cryptography;
using CummonTestUtilities.Entities;
using CummonTestUtilities.Repositories;
using CummonTestUtilities.Requests;
using CummonTestUtilities.Token;
using Shouldly;

namespace UseCase.Test.Login.DoLogin
{
    public class DoLoginUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var user = UserBuilder.Build();
            var request = RequestLoginJsonBuilder.Build();
            request.Email = user.Email;

            var useCase = CreateUseCase(user, request.Password);

            //Act
            var result = await useCase.Execute(request);

            //Assign
            result.ShouldNotBeNull();
            result.Name.ShouldBe(user.Name);
            result.Token.ShouldNotBeNull();
        }

        [Fact]
        public async Task ErrorUserNotFound()
        {
            //Arrange
            var user = UserBuilder.Build();
            var request = RequestLoginJsonBuilder.Build();
            var useCase = CreateUseCase(user, request.Password);

            //Act
            var act = async () => await useCase.Execute(request);
            var result = await act.ShouldThrowAsync<InvalidLoginException>();

            //Assign
            result.GetErrors().Count.ShouldBe(1);
            result.GetErrors().ShouldContain(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID);
        }

        [Fact]
        public async Task ErrorPasswordNotMatch()
        {
            //Arrange
            var user = UserBuilder.Build();
            var request = RequestLoginJsonBuilder.Build();
            request.Email = user.Email;
            var useCase = CreateUseCase(user);

            //Act
            var act = async () => await useCase.Execute(request);
            var result = await act.ShouldThrowAsync<InvalidLoginException>();

            //Assign
            result.GetErrors().Count.ShouldBe(1);
            result.GetErrors().ShouldContain(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID);
        }

        private DoLoginUseCase CreateUseCase(User user, string? password = null)
        {
            var passwordEncripter = new PasswordEncrypterBuilder().Verify(password).Build();
            var tokenGenerator = JwtTokenGeneratorBuilder.Builder();
            var readRepository = new UserReadOnlyRepositoryBuilder().GetUserByEmail(user).Build();

            return new DoLoginUseCase(readRepository, passwordEncripter, tokenGenerator);
        }
    }
}
