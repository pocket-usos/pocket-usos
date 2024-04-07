using App.API.UserAccess.Requests;
using App.Application.Configuration;
using App.Application.Contracts;
using App.Application.UserAccess.Authentication.Authenticate;
using App.Application.UserAccess.Authentication.InitialiseAuthenticationSession;
using App.Application.UserAccess.Authentication.LogOut;
using Microsoft.AspNetCore.Mvc;

namespace App.API.UserAccess;

[ApiController]
[Route("authentication")]
public class AuthenticationController(IGateway gateway, IExecutionContextAccessor context) : ControllerBase
{
    [HttpPost("sessions")]
    [ProducesResponseType(typeof(AuthenticationSessionInitialisationResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> InitialiseSession([FromQuery] InitialiseAuthenticationSessionRequest request)
    {
        var result = await gateway.ExecuteCommandAsync(new InitialiseAuthenticationSessionCommand(request.InstitutionId));

        return Ok(result);
    }

    [HttpPatch("sessions/{sessionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Authenticate(Guid sessionId, AuthenticateRequest request)
    {
        await gateway.ExecuteCommandAsync(new AuthenticateCommand(sessionId, request.RequestToken, request.Verifier));

        return Ok();
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> LogOut()
    {
        await gateway.ExecuteCommandAsync(new LogOutCommand(context.SessionId));

        return Ok();
    }
}
