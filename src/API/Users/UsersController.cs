using App.API.Users.Requests;
using App.Application.Contracts;
using App.Application.Users.GetMyProfile;
using App.Application.Users.GetUser;
using App.Application.Users.GetUsersPhotos;
using App.Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Users;

[ApiController]
[Route("users")]
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

    [HttpGet("photos")]
    [ProducesResponseType(typeof(IDictionary<string, string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsersPhotos(GetUsersPhotosRequest request)
    {
        var photos = await gateway.ExecuteQueryAsync(new GetUsersPhotosQuery(request.UsersIds));

        return Ok(photos);
    }
}
