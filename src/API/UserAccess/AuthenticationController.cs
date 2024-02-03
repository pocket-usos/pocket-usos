using App.API.UserAccess.Requests;
using App.Application.Contracts;
using App.Application.UserAccess.Authentication.Authenticate;
using App.Application.UserAccess.Authentication.InitialiseAuthenticationSession;
using Microsoft.AspNetCore.Mvc;

namespace App.API.UserAccess;

[ApiController]
[Route("authentication")]
public class AuthenticationController(IGateway gateway) : ControllerBase
{
    [HttpPost("sessions")]
    [ProducesResponseType(typeof(AuthenticationSessionInitialisationResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> InitialiseSession()
    {
        var result = await gateway.ExecuteCommandAsync(new InitialiseAuthenticationSessionCommand());
        
        return Ok(result);
    }
    
    [HttpPatch("sessions/{sessionId}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Authenticate(Guid sessionId, AuthenticateRequest request)
    {
        await gateway.ExecuteCommandAsync(new AuthenticateCommand(sessionId, request.RequestToken, request.Verifier));
        
        return Ok();
    }
}
