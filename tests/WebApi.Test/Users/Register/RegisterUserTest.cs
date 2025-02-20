using CashFlow.Exception;
using CummonTestUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Register
{
    public class RegisterUserTest : CashFlowClassFixture
    {
        private const string METHOD = "User";
        public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory) { }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var request = RequestRegisterUserJsonBuilder.Build();

            //Act
            var result = await DoPost(METHOD, request);
            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);

            //Assert
            result.StatusCode.ShouldBe(HttpStatusCode.Created);
            response.RootElement.GetProperty("name").GetString().ShouldBe(request.Name);
            response.RootElement.GetProperty("token").GetString().ShouldNotBeNullOrWhiteSpace();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task ErrorEmptyName(string culture)
        {
            //Arrange
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            //Act
            var result = await DoPost(uri: METHOD,request: request, culture: culture);
            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);
            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

            //Assert
            result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            errors.ShouldSatisfyAllConditions(
                () => errors.ShouldHaveSingleItem(),
                () => errors.ShouldContain(error => error.GetString()!.Equals(expectedMessage))
            );
        }
    }
}
