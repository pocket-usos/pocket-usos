using App.API.Grades.Requests;
using App.Application.Contracts;
using App.Application.Grades.GetTermGrades;
using App.Domain.Grades;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Grades;

[ApiController]
[Route("grades")]
public class GradesController(IGateway gateway) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(TermGrades), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTermGrades([FromQuery] GetTermGradesRequest request)
    {
        var termGrades = await gateway.ExecuteQueryAsync(new GetTermGradesQuery(request.Term));

        return Ok(termGrades);
    }
}
