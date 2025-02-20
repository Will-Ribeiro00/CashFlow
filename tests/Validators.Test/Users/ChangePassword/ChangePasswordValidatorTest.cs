using CashFlow.Application.UseCases.Users.ChangePassword;
using CashFlow.Communication.Requests.RequestUserJson;
using CashFlow.Exception;
using CummonTestUtilities.Requests;
using Shouldly;

namespace Validators.Test.Users.ChangePassword
{
    public class ChangePasswordValidatorTest
    {
        [Fact]
        public void Success()
        {
            //Arrange
            var validator = new ChangePasswordValidator();
            var request = RequestChangePasswordJsonBuilder.Build();

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.ShouldBeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void ErrorNewPasswordEmpty(string password)
        {
            //Arrange
            var validator = new ChangePasswordValidator();
            var request = RequestChangePasswordJsonBuilder.Build();
            request.NewPassword = string.Empty;

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.ShouldSatisfyAllConditions(
                () => result.Errors.Count.ShouldBe(1),
                () => result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceErrorMessages.INVALID_PASSWORD))
                );
        }
    }
}
