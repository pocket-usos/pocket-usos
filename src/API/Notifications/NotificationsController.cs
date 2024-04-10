using App.API.Configuration.UsosAuthorize;
using App.API.Notifications.Requests;
using App.Application.Configuration;
using App.Application.Contracts;
using App.Application.Notifications;
using App.Application.Notifications.GetNotifications;
using App.Application.Notifications.GetOneSignalExternalId;
using App.Application.Notifications.ReadNotifications;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Notifications;

[ApiController]
[UsosAuthorize]
[Route("notifications")]
public class NotificationsController(IGateway gateway, IExecutionContextAccessor context) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<NotificationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNotifications()
    {
        var notifications = await gateway.ExecuteQueryAsync(new GetNotificationsQuery());

        return Ok(notifications);
    }

    [HttpPut("read")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ReadNotifications(ReadNotificationsRequest request)
    {
        await gateway.ExecuteCommandAsync(new ReadNotificationsCommand(request.NotificationIds));

        return Ok();
    }

    [HttpGet("external-id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOneSignalExternalId()
    {
        var externalId = await gateway.ExecuteQueryAsync(new GetOneSignalExternalIdQuery(context.SessionId));

        return Ok(new
        {
            Id = externalId
        });
    }
}
