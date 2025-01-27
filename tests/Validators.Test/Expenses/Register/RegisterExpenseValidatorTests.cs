using CashFlow.Application.UseCases.Expenses;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CummonTestUtilities.Requests;
using Shouldly;

namespace Validators.Test.Expenses.Register
{
    public class RegisterExpenseValidatorTests
    {
        [Fact]
        public void Success()
        {
            //Arrange
            var validator = new ExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void ErrorDateFuture()
        {
            //Arrange
            var validator = new ExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            request.Date = DateTime.UtcNow.AddDays(+1);

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceErrorMessages.EXPENSE_CANNOT_BE_FOR_THE_FUTURE);
        }

        [Fact]
        public void ErrorPaymentTypeInvalid()
        {
            //Arrange
            var validator = new ExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            var invalidValue = new Random();
            request.PaymentType = (PaymentType)invalidValue.Next(4, 100);

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceErrorMessages.PAYMENTS_TYPE_INVALID);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(-100)]
        public void ErrorAmountLessThanZero(decimal amount)
        {
            //Arrange
            var validator = new ExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            request.Amount = amount;

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO);
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        public void ErrorTitleEmpty(string title)
        {
            //Arrange
            var validator = new ExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            request.Title = title;

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceErrorMessages.TITLE_REQUIRED);
        }
    }
}
