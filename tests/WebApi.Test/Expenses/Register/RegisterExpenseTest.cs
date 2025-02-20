using CashFlow.Exception;
using CummonTestUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.Register
{
    public class RegisterExpenseTest : CashFlowClassFixture
    {
        private const string METHOD = "Expenses";

        private readonly string _token;

        public RegisterExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var request = RequestExpenseJsonBuilder.Build();


            //Act
            var result = await DoPost(uri: METHOD, request: request, token: _token);
            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);

            //Assert 
            result.StatusCode.ShouldBe(HttpStatusCode.Created);
            response.RootElement.GetProperty("title").GetString().ShouldBe(request.Title);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task ErrorTitleEmpty(string culture)
        {
            //Arrange
            var request = RequestExpenseJsonBuilder.Build();
            request.Title = string.Empty;

            //Act
            var result = await DoPost(uri: METHOD, request: request, token: _token, culture: culture);
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
