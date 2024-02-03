using App.Application.Contracts;
using App.Application.Users.GetMyProfile;
using App.Application.Users.GetUser;
using App.Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Users;

[ApiController]
[Route("students")]
public class UsersController(IGateway gateway) : ControllerBase
{
    [HttpGet("me")]
    [ProducesResponseType(typeof(Profile), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyProfile()
    {
        var profile = await gateway.ExecuteQueryAsync(new GetMyProfileQuery());

        return Ok(profile);
    }

    [HttpGet("{studentId}")]
    [ProducesResponseType(typeof(Profile), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUser(string studentId)
    {
        var student = await gateway.ExecuteQueryAsync(new GetUserQuery(studentId));

        return Ok(student);
    }
}
