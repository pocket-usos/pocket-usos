using App.API.Configuration.UsosAuthorize;
using App.API.Grades.Requests;
using App.Application.Contracts;
using App.Application.Grades;
using App.Application.Grades.GetGradesDistribution;
using App.Application.Grades.GetTermGrades;
using App.Application.Grades.NotifyAboutNewGrade;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Grades;

[ApiController]
[Route("grades")]
public class GradesController(IGateway gateway) : ControllerBase
{
    [HttpGet]
    [UsosAuthorize]
    [ProducesResponseType(typeof(TermGrades), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTermGrades([FromQuery] GetTermGradesRequest request)
    {
        var termGrades = await gateway.ExecuteQueryAsync(new GetTermGradesQuery(request.Term));

        return Ok(termGrades);
    }

    [HttpGet("{examId}/distribution")]
    [UsosAuthorize]
    [ProducesResponseType(typeof(GradesDistributionItem[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGradesDistribution(string examId)
    {
        var distribution = await gateway.ExecuteQueryAsync(new GetGradesDistributionQuery(examId));

        return Ok(distribution);
    }

    [HttpPost("{institutionId}")]
    public async Task<IActionResult> NewGradeEventListener(Guid institutionId, [FromBody] NewGradeEvent @event)
    {
        await gateway.ExecuteCommandAsync(new NotifyAboutNewGradeCommand(institutionId, @event.Entry.Select(e => new NewGradeNotificationEntry
        {
            TimeStamp = e.Time,
            RelatedUserIds = e.Related_User_Ids,
            ExamId = e.Exam_Id,
            ExamSessionNumber = e.Exam_Session_Number

        }).ToArray()));

        return Ok();
    }
}
