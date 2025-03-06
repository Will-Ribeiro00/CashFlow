using CashFlow.Application.UseCases.Login.DoLogin;
using CashFlow.Communication.Requests.RequestUserJson;
using CashFlow.Communication.Responses.ResponseErrorJson;
using CashFlow.Communication.Responses.ResponseUserJson;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    public class LoginController : CashFlowBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromServices] IDoLoginUseCase useCase,
                                               [FromBody] RequestLoginJson request)
        {
            var response = await useCase.Execute(request);

            return Ok(response);
        }
    }
}
