using CashFlow.Communication.Requests.RequestUserJson;
using CashFlow.Exception;
using CummonTestUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.ChangePassword
{
    public class ChangePasswordTest : CashFlowClassFixture
    {
        private const string METHOD = "User/change-password";

        private readonly string _token; 
        private readonly string _password; 
        private readonly string _email; 
        public ChangePasswordTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _password = webApplicationFactory.User_Team_Member.GetPassword();
            _email = webApplicationFactory.User_Team_Member.GetEmail();
        }

        [Fact]
        public async Task Success()
        {
            //1º Teste alterando a senha
            var request = RequestChangePasswordJsonBuilder.Build();
            request.Password = _password;

            var response = await DoPut(METHOD, request, _token);
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

            //2º Teste tentando se loggar com a senha antiga
            var loginRequest = new RequestLoginJson
            {
                Email = _email,
                Password = _password
            };

            response = await DoPost("Login", loginRequest);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            //3º Teste se loggando com a senha nova
            loginRequest.Password = request.NewPassword;
            response = await DoPost("Login", loginRequest);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task ErrorPasswordDifferentCurrentPassword(string culture)
        {
            var request = RequestChangePasswordJsonBuilder.Build();

            var response = await DoPut(METHOD, request, _token, culture);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);
            var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("PASSWORD_DIFFERENT_CURRENT_PASSWORD", new CultureInfo(culture));

            errors.ShouldHaveSingleItem();
            errors.ShouldContain(e => e.GetString()!.Equals(expectedMessage));
        }
    }
}
