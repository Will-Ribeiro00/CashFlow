using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CummonTestUtilities.Cryptography;
using CummonTestUtilities.Mapper;
using CummonTestUtilities.Repositories;
using CummonTestUtilities.Requests;
using CummonTestUtilities.Token;
using Shouldly;

namespace UseCase.Test.Users.Register
{
    public class RegisterUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var request = RequestRegisterUserJsonBuilder.Build();
            var useCase = CreateUseCase();

            //Act
            var result = await useCase.Execute(request);

            //Assign
            result.ShouldNotBeNull();
            result.Name.ShouldBeEquivalentTo(request.Name);
            result.Token.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public async Task ErrorNameEmpty()
        {
            //Arrange
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;
            var useCase = CreateUseCase();

            //Act
            var act = async () => await useCase.Execute(request);
            var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

            //Assign
            result.GetErrors().Count.ShouldBe(1);
            result.GetErrors().ShouldContain(ResourceErrorMessages.NAME_EMPTY);
        }

        [Fact]
        public async Task ErrorEmailAlreadyExist()
        {
            //Arrange
            var request = RequestRegisterUserJsonBuilder.Build();
            var useCase = CreateUseCase(request.Email);

            //Act
            var act = async () => await useCase.Execute(request);
            var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

            //Assign
            result.GetErrors().Count.ShouldBe(1);
            result.GetErrors().ShouldContain(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED);
        }

        private RegisterUserUseCase CreateUseCase(string? email = null)
        {
            var mapper = MapperBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
            var passwordEncripter = new PasswordEncrypterBuilder().Build();
            var tokenGenerator = JwtTokenGeneratorBuilder.Builder();
            var readRepository = new UserReadOnlyRepositoryBuilder();

            if (!string.IsNullOrWhiteSpace(email))
                readRepository.ExistActiveUserWithEmail(email);

            return new RegisterUserUseCase(mapper, passwordEncripter, readRepository.Build(), writeRepository, tokenGenerator, unitOfWork);
        }
    }
}
