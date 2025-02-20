using CashFlow.Communication.Requests.RequestUserJson;
using Shouldly;
using System.Net;

namespace WebApi.Test.Users.Delete
{
    public class DeleteUserAccountTest : CashFlowClassFixture
    {
        private const string METHOD = "User";

        private readonly string _token;
        private readonly string _email;
        private readonly string _password;
        public DeleteUserAccountTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _email = webApplicationFactory.User_Team_Member.GetEmail();
            _password = webApplicationFactory.User_Team_Member.GetPassword();
        }

        [Fact]
        public async Task Success()
        {
            var result = await DoDelete(METHOD, _token);

            result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

            var requestLogin = new RequestLoginJson
            {
                Email = _email,
                Password = _password
            };

            result = await DoPost($"Login", requestLogin, _token);

            result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
    }
}
