using CashFlow.Exception;
using CummonTestUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Update
{
    public class UpdateUserTest : CashFlowClassFixture
    {
        private const string METHOD = "User";
        private readonly string _token;
        public UpdateUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange and Act
            var request = RequestUpdateUserJsonBuilder.Build();
            var response = await DoPut(uri: $"{METHOD}", request, token: _token);

            //Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task ErrorEmptyName(string culture)
        {
            //Arrange and Act
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;
            var response = await DoPut(uri: $"{METHOD}", request, token: _token, culture: culture);
            var body = await response.Content.ReadAsStreamAsync();
            var data = await JsonDocument.ParseAsync(body);

            var erros = data.RootElement.GetProperty("errorMessages").EnumerateArray();
            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

            //Assert
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            erros.ShouldSatisfyAllConditions(
                () => erros.ShouldHaveSingleItem(),
                () => erros.ShouldContain(cultureErro => cultureErro.GetString()!.Equals(expectedMessage))
                );
        }
    }
}
