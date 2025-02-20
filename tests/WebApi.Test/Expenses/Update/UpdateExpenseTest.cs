using CashFlow.Exception;
using CummonTestUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.Update
{
    public class UpdateExpenseTest : CashFlowClassFixture
    {
        private const string METHOD = "Expenses";

        private string _token;
        private int _expenseId;
        public UpdateExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _expenseId = webApplicationFactory.Expense_Member_Team.GetExpenseId();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var request = RequestExpenseJsonBuilder.Build();

            //Act
            var result = await DoPut(uri: $"{METHOD}/{_expenseId}", request: request, token: _token);

            //Assert
            result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task ErrorTitleEmpty(string culture)
        {
            //Arrange
            var request = RequestExpenseJsonBuilder.Build();
            request.Title = string.Empty;

            //Act
            var result = await DoPut(uri: $"{METHOD}/{_expenseId}", request: request, token: _token, culture: culture);
            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);
            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(culture));

            //Assert
            result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            errors.ShouldSatisfyAllConditions(
                () => errors.ShouldHaveSingleItem(),
                () => errors.ShouldContain(error => error.GetString()!.Equals(expectedMessage))
            );
        }
    }
}
