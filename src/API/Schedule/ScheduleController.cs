using App.API.Schedule.Requests;
using App.Application.Contracts;
using App.Application.Schedule;
using App.Application.Schedule.GetMySchedule;
using App.Application.Schedule.GetMyTerms;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Schedule;

[ApiController]
[Route("schedule")]
public class ScheduleController(IGateway gateway) : ControllerBase
{
    [HttpGet("my")]
    [ProducesResponseType(typeof(IEnumerable<ScheduleItem>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMySchedule([FromQuery] GetMyScheduleRequest request)
    {
        var schedule = await gateway.ExecuteQueryAsync(new GetMyScheduleQuery(request.Start, request.Days));

        return Ok(schedule);
    }

    [HttpGet("terms")]
    [ProducesResponseType(typeof(IEnumerable<Term>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyTerms()
    {
        var terms = await gateway.ExecuteQueryAsync(new GetMyTermsQuery());

        return Ok(terms);
    }
}
