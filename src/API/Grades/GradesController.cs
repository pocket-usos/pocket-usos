using App.API.Configuration.UsosAuthorize;
using App.API.Grades.Requests;
using App.Application.Contracts;
using App.Application.Grades;
using App.Application.Grades.GetGradesDistribution;
using App.Application.Grades.GetTermGrades;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Grades;

[ApiController]
[UsosAuthorize]
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

    [HttpGet("{examId}/distribution")]
    [ProducesResponseType(typeof(GradesDistributionItem[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGradesDistribution(string examId)
    {
        var distribution = await gateway.ExecuteQueryAsync(new GetGradesDistributionQuery(examId));

        return Ok(distribution);
    }
}
