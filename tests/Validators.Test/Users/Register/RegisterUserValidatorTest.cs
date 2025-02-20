using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CummonTestUtilities.Requests;
using Shouldly;

namespace Validators.Test.Users.Register
{
    public class RegisterUserValidatorTest 
    {
        [Fact]
        public void Success()
        {
            //Arrange
            var validator = new RegisterUserValidator();
            var request = RequestRegisterUserJsonBuilder.Build();

            //Act
            var result = validator.Validate(request);

            //Arrange
            result.IsValid.ShouldBeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void ErrorNameEmpty(string name)
        {
            //Arrange
            var validator = new RegisterUserValidator();
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = name;

            //Act
            var result = validator.Validate(request);

            //Arrange
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceErrorMessages.NAME_EMPTY);
        }

        [Fact]
        public void ErrorPasswordEmpty()
        {
            //Arrange
            var validator = new RegisterUserValidator();
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Password = string.Empty;

            //Act
            var result = validator.Validate(request);

            //Arrange
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceErrorMessages.INVALID_PASSWORD);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void ErrorEmailEmpty(string email)
        {
            //Arrange
            var validator = new RegisterUserValidator();
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Email = email;

            //Act
            var result = validator.Validate(request);

            //Arrange
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceErrorMessages.EMAIL_EMPTY);
        }

        [Fact]
        public void ErrorEmailInvalid()
        {
            //Arrange
            var validator = new RegisterUserValidator();
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Email = "teste.com";

            //Act
            var result = validator.Validate(request);

            //Arrange
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceErrorMessages.EMAIL_INVALID);
        }

    }
}
