using CashFlow.Communication.Requests.RequestUserJson;
using CashFlow.Exception;
using CummonTestUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin
{
    public class DoLoginTest : CashFlowClassFixture
    {
        private const string METHOD = "Login";

        private readonly string _email;
        private readonly string _name;
        private readonly string _password;
        public DoLoginTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _email = webApplicationFactory.User_Team_Member.GetEmail();
            _name = webApplicationFactory.User_Team_Member.GetName();
            _password = webApplicationFactory.User_Team_Member.GetPassword();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var request = new RequestLoginJson
            {
                Email = _email,
                Password = _password
            };

            //Act
            var result = await DoPost(uri: METHOD, request: request);
            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);

            //Assert
            response.RootElement.GetProperty("name").GetString().ShouldBe(_name);
            response.RootElement.GetProperty("token").GetString().ShouldNotBeNullOrWhiteSpace();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task ErrorLoginInvalid(string culture)
        {
            //Arrange
            var request = RequestLoginJsonBuilder.Build();

            //Act
            var result = await DoPost(uri: METHOD, request: request, culture: culture);
            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);
            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

            //Assert
            result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
            errors.ShouldSatisfyAllConditions(
                () => errors.ShouldHaveSingleItem(),
                () => errors.ShouldContain(error => error.GetString()!.Equals(expectedMessage))
            );
        }
    }
}
