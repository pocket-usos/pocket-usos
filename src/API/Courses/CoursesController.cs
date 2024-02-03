using App.API.Courses.Requests;
using App.Application.Contracts;
using App.Application.Courses.GetCoursesForTerm;
using App.Domain.Courses;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Courses;

[ApiController]
[Route("courses")]
public class CoursesController(IGateway gateway) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Course>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTermGrades([FromQuery] GetTermCoursesRequest request)
    {
        var termGrades = await gateway.ExecuteQueryAsync(new GetCoursesForTermQuery(request.Term));

        return Ok(termGrades);
    }
}
