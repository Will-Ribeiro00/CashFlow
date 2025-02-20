using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Users.Profile
{
    public class GetUserProfileTest : CashFlowClassFixture
    {
        private const string METHOD = "User";


        private readonly string _token;
        private readonly string _UserName;
        private readonly string _UserEmail;

        public GetUserProfileTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _UserName = webApplicationFactory.User_Team_Member.GetName();
            _UserEmail = webApplicationFactory.User_Team_Member.GetEmail();
        }


        [Fact]
        public async Task Success()
        {
            //Arrange and Act
            var result = await DoGet(uri: $"{METHOD}", token: _token);
            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);

            //Assign
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
            response.RootElement.GetProperty("name").GetString().ShouldBe(_UserName);
            response.RootElement.GetProperty("email").GetString().ShouldBe(_UserEmail);
        }
    }
}
