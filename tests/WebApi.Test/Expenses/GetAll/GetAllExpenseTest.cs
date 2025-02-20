using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Expenses.GetAll
{
    public class GetAllExpenseTest : CashFlowClassFixture
    {
        private const string METHOD = "Expenses";

        private readonly string _token;

        public GetAllExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var result = await DoGet(uri: METHOD, token: _token);
            var body = await result.Content.ReadAsStreamAsync();
            
            //Act
            var response = await JsonDocument.ParseAsync(body);

            //Assert
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
            response.RootElement.GetProperty("expenses").EnumerateArray().ShouldNotBeEmpty();
        }
    }
}
