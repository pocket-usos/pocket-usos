using App.API.Calendar.Requests;
using App.Application.Calendar.GetMyCalendar;
using App.Application.Contracts;
using App.Domain.Calendar;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Calendar;

[ApiController]
[Route("calendar")]
public class CalendarController(IGateway gateway) : ControllerBase
{
    [HttpGet("my")]
    [ProducesResponseType(typeof(IEnumerable<CalendarItem>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyCalendar([FromQuery] GetMyCalendarRequest request)
    {
        var calendar = await gateway.ExecuteQueryAsync(new GetMyCalendarQuery(request.Start, request.Days));

        return Ok(calendar);
    }
}
