using App.API.Calendar.Requests;
using App.Application.Calendar.GetMyCalendar;
using App.Application.Calendar.GetMyTerms;
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

    [HttpGet("terms")]
    [ProducesResponseType(typeof(IEnumerable<Term>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyTerms()
    {
        var terms = await gateway.ExecuteQueryAsync(new GetMyTermsQuery());

        return Ok(terms);
    }
}
