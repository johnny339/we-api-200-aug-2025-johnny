

using Marten;
using Microsoft.AspNetCore.SignalR;

namespace HelpDesk.Api.Employee.Issues;

[ApiController]
public class EmployeeIssueController : ControllerBase
{
    [HttpPost("/employee/problems")]
    public async Task<ActionResult> AddEmployeeProblemAsync(
        [FromBody] SubmitIssueRequest request,
        [FromServices] TimeProvider clock,
        [FromServices] IProvideUserInfo userService,
        [FromServices] IDocumentSession session,
        CancellationToken token
        )
    {
        // we should validate the incoming request against the "rules"
        // - Field level stuff - did they send everything, does it meet the rules, etc. (FluentValidation)
        // - SoftwareId - 

        string userSub = await userService.GetUserSubAsync(token);
        // "Slime" (BS)
        var response = new SubmitIssueResponse
        {
            Id = Guid.NewGuid(), // Maybe slime? has to be the database id?
            ReportedAt = clock.GetLocalNow(),
            ReportedBy = userSub, // Slime. Fake - this has to come from authorization.
            ReportedProblem = request,
            Status = SubmittedIssueStatus.AwaitingTechAssignment
        };
        var entity = response.MapToEntity();
        session.Store(entity);
        await session.SaveChangesAsync();
        // save this thing somewhere. 
        // Slime, too - because you can't GET that location and get the same response.
        return Created($"/employee/problems/{response.Id}", response);
    }

    [HttpGet("/employee/problems/{problemId:guid}")]
    public async Task<ActionResult> GetProblemByIdAsync(Guid problemId,
        [FromServices]IDocumentSession session,
        CancellationToken token)
    {
        var entity = await session.Query<EmployeeProblemEntity>().SingleOrDefaultAsync(p => p.Id == problemId);

        if(entity is null)
        {
            return NotFound();
        } else
        {
            var response = entity.MapToResponse();
            return Ok(response);
        }
    }
 }
