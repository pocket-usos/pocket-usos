using App.API.Courses.Requests;
using App.Application.Contracts;
using App.Application.Courses;
using App.Application.Courses.GetCoursesForTerm;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Courses;

[ApiController]
[Route("courses")]
public class CoursesController(IGateway gateway) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CourseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCoursesForTerm([FromQuery] GetTermCoursesRequest request)
    {
        var courses = await gateway.ExecuteQueryAsync(new GetCoursesForTermQuery(request.Term));

        return Ok(courses);
    }
}
