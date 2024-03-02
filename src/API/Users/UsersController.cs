using App.API.Users.Requests;
using App.Application.Contracts;
using App.Application.Users;
using App.Application.Users.GetMyProfile;
using App.Application.Users.GetUser;
using App.Application.Users.GetUsers;
using App.Application.Users.GetUsersPhotos;
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

    [HttpGet("{userId}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUser(string userId)
    {
        var user = await gateway.ExecuteQueryAsync(new GetUserQuery(userId));

        return Ok(user);
    }

    [HttpGet]
    [ProducesResponseType(typeof(User[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersRequest request)
    {
        var users = await gateway.ExecuteQueryAsync(new GetUsersQuery(request.UsersIds.Split(',')));

        return Ok(users);
    }

    [HttpGet("photos")]
    [ProducesResponseType(typeof(IDictionary<string, string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsersPhotos([FromQuery] GetUsersPhotosRequest request)
    {
        var photos = await gateway.ExecuteQueryAsync(new GetUsersPhotosQuery(request.UsersIds.Split(',')));

        return Ok(photos);
    }
}
