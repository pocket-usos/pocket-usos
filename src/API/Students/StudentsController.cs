using App.Application.Contracts;
using App.Application.Students.GetMyProfile;
using App.Application.Students.GetStudent;
using App.Domain.Students;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Students;

[ApiController]
[Route("students")]
public class StudentsController(IGateway gateway) : ControllerBase
{
    [HttpGet("me")]
    [ProducesResponseType(typeof(Profile), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyProfile()
    {
        var profile = await gateway.ExecuteQueryAsync(new GetMyProfileQuery());

        return Ok(profile);
    }

    [HttpGet("{studentId}")]
    [ProducesResponseType(typeof(Profile), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudent(string studentId)
    {
        var student = await gateway.ExecuteQueryAsync(new GetStudentQuery(studentId));

        return Ok(student);
    }
}
