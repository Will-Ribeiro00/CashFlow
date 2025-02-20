using CashFlow.Application.UseCases.Users;
using CashFlow.Communication.Requests.RequestUserJson;
using FluentValidation;
using Shouldly;

namespace Validators.Test.Users
{
    public class PasswordValidatorTest
    {
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("abc")]
        [InlineData("abcd")]
        [InlineData("abcde")]
        [InlineData("abcdef")]
        [InlineData("abcdefg")]
        [InlineData("abcdefgh")]
        [InlineData("ABCDEFGH")]
        [InlineData("Abcdefgh1")]
        public void ErrorsPasswordInvalid(string password)
        {
            //Arrange
            var validator = new PasswordValidator<RequestRegisterUserJson>();

            //Act
            var result = validator.IsValid(new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson()), password: password);

            //Arrange
            result.ShouldBeFalse();
        }
    }
}
