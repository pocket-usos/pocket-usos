using App.Application.Contracts;
using App.Application.Institutions.Public;
using App.Application.Institutions.Public.GetInstitutions;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Institutions;

[ApiController]
[Route("institutions")]
public class InstitutionsController(IGateway gateway) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InstitutionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList()
    {
        var institutions = await gateway.ExecuteQueryAsync(new GetInstitutionsQuery());

        return Ok(institutions);
    }
}
