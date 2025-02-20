using CashFlow.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.Delete
{
    public class DeleteExpenseTest : CashFlowClassFixture
    {
        private const string METHOD = "Expenses";

        private readonly string _token;
        private readonly int _expenseId;
        public DeleteExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _expenseId = webApplicationFactory.Expense_Member_Team.GetExpenseId();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange - Act
            var resultDelete = await DoDelete(uri: $"{METHOD}/{_expenseId}", token: _token);
            var resultGet = await DoGet(uri: $"{METHOD}/{_expenseId}", token: _token);

            //Assert
            resultDelete.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            resultGet.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task ErrorExpenseNotFound(string culture)
        {
            //Arrange - Act
            var result = await DoDelete(uri: $"{METHOD}/1000000", token: _token, culture: culture);
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
