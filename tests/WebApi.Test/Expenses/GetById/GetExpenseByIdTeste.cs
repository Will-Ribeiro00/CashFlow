using CashFlow.Communication.Enums;
using CashFlow.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.GetById
{
    public class GetExpenseByIdTeste : CashFlowClassFixture
    {
        private const string METHOD = "Expenses";

        private readonly string _token;
        private readonly int _expenseId;
        public GetExpenseByIdTeste(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _expenseId = webApplicationFactory.Expense_Member_Team.GetExpenseId();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange - Act
            var result = await DoGet(uri: $"{METHOD}/{_expenseId}", token: _token);
            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);

            //Assert
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
            response.ShouldSatisfyAllConditions(
                () => response.ShouldNotBeNull(),
                () => response.RootElement.GetProperty("id").GetInt64().ShouldBe(_expenseId),
                () => response.RootElement.GetProperty("title").GetString().ShouldNotBeNullOrWhiteSpace(),
                () => response.RootElement.GetProperty("description").GetString().ShouldNotBeNullOrWhiteSpace(),
                () => response.RootElement.GetProperty("date").GetDateTime().ShouldBeLessThanOrEqualTo(DateTime.Now),
                () => response.RootElement.GetProperty("amount").GetDecimal().ShouldBeGreaterThan(0),
                () => response.RootElement.GetProperty("tags").EnumerateArray().ShouldNotBeEmpty(),
                () => Enum.IsDefined(typeof(PaymentType), response.RootElement.GetProperty("paymentType").GetInt32()).ShouldBeTrue()
                );
        }
        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task ErrorExpenseNotFound(string culture)
        {
            //Arrange - Act
            var result = await DoGet(uri: $"{METHOD}/1000000", token: _token, culture: culture);
            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);
            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(culture));

            //Assert
            result.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            errors.ShouldSatisfyAllConditions(
                () => errors.ShouldHaveSingleItem(),
                () => errors.ShouldContain(error => error.GetString()!.Equals(expectedMessage))
            );
        }
    }
}
