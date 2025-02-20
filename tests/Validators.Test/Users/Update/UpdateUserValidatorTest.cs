using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Exception;
using CummonTestUtilities.Requests;
using Shouldly;

namespace Validators.Test.Users.Update
{
    public class UpdateUserValidatorTest
    {
        [Fact]
        public void Success()
        {
            //Arrange
            var validator = new UpdateUserValidator();
            var request = RequestUpdateUserJsonBuilder.Build();

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.ShouldBeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        public void ErrorNameEmpry(string name)
        {
            //Arrange
            var validator = new UpdateUserValidator();
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = name;

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.ShouldSatisfyAllConditions(
                () => result.Errors.ShouldHaveSingleItem(),
                () => result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY))
                );
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        public void ErrorEmailEmpry(string name)
        {
            //Arrange
            var validator = new UpdateUserValidator();
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = name;

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.ShouldSatisfyAllConditions(
                () => result.Errors.ShouldHaveSingleItem(),
                () => result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY))
                );
        }
        [Fact]
        public void ErrorEmailInvalid()
        {
            //Arrange
            var validator = new UpdateUserValidator();
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = "Teste_Email_Invalido.com";

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.ShouldSatisfyAllConditions(
                () => result.Errors.ShouldHaveSingleItem(),
                () => result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID))
                );
        }
    }
}
