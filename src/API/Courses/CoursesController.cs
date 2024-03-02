using App.API.Courses.Requests;
using App.Application.Contracts;
using App.Application.Courses;
using App.Application.Courses.GetCourse;
using App.Application.Courses.GetCoursesForTerm;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Courses;

[ApiController]
[Route("courses")]
public class CoursesController(IGateway gateway) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Course>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCoursesForTerm([FromQuery] GetCoursesForTermRequest request)
    {
        var courses = await gateway.ExecuteQueryAsync(new GetCoursesForTermQuery(request.Term, request.WithSchedule));

        return Ok(courses);
    }

    [HttpGet("{courseId}")]
    [ProducesResponseType(typeof(Course), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCourse(string courseId, [FromQuery] GetCourseRequest request)
    {
        var course = await gateway.ExecuteQueryAsync(new GetCourseQuery(courseId, request.CourseUnitId));

        return Ok(course);
    }
}
