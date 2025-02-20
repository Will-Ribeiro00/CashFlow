using Shouldly;
using System.Net;
using System.Net.Mime;

namespace WebApi.Test.Expenses.Reports
{
    public class GenerateExpensesReportTest : CashFlowClassFixture
    {
        private const string METHOD = "Report";

        private readonly string _adminToken;
        private readonly string _teamMemeberToken;
        private readonly DateTime _expenseDate;

        public GenerateExpensesReportTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _adminToken = webApplicationFactory.User_Admin.GetToken();
            _teamMemeberToken = webApplicationFactory.User_Team_Member.GetToken();
            _expenseDate = webApplicationFactory.Expense_Admin.GetDate();
        }

        [Fact]
        public async Task SuccessPdf()
        {
            //Arrange Act
            var result = await DoGet(uri: $"{METHOD}/PDF?date={_expenseDate:yyyy-MM}", token: _adminToken);

            //Assert
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
            result.Content.Headers.ContentType.ShouldNotBeNull();
            result.Content.Headers.ContentType!.MediaType.ShouldBe(MediaTypeNames.Application.Pdf);
        }
        [Fact]
        public async Task SuccessExcel()
        {
            //Arrange Act
            var result = await DoGet(uri: $"{METHOD}/Excel?date={_expenseDate:yyyy-MM}", token: _adminToken);

            //Assert
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
            result.Content.Headers.ContentType.ShouldNotBeNull();
            result.Content.Headers.ContentType!.MediaType.ShouldBe(MediaTypeNames.Application.Octet);
        }
        [Fact]
        public async Task ErrorForbiddenUserNotAllowedPdf()
        {
            //Arrange and Act
            var result = await DoGet(uri: $"{METHOD}/pdf?date={_expenseDate:Y}", token: _teamMemeberToken);

            //Assert
            result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }
        [Fact]
        public async Task ErrorForbiddenUserNotAllowedExcel()
        {
            //Arrange and Act
            var result = await DoGet(uri: $"{METHOD}/excel?date={_expenseDate:Y}", token: _teamMemeberToken);

            //Assert
            result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }
    }
}

