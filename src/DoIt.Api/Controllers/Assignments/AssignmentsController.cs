using DoIt.Api.Controllers._Common;
using DoIt.Api.Services.Assignments;
using Microsoft.AspNetCore.Mvc;

namespace DoIt.Api.Controllers.Assignments;

[Route("api/assignments")]
public class AssignmentsController(IAssignmentsService assignmentsService)
    : ApiController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var assignments = await assignmentsService.GetAll();
        return Ok(assignments);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await assignmentsService.GetById(id);
        
        return result.Map(
            onSuccess: Ok,
            onFailure: Problem
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAssignmentRequest request)
    {
        var result = await assignmentsService.Create(request);

        return result.Map(
            onSuccess: (createdAssignment)
                => CreatedAtAction(
                    actionName: nameof(GetById),
                    routeValues: new { id =  createdAssignment.Id },
                    value: null
                ),
            onFailure: Problem
        );
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await assignmentsService.Delete(id);

        return result.Map(
            onSuccess: NoContent,
            onFailure: Problem
        );
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateAssignmentRequest request
    )
    {
        var result = await assignmentsService.Update(id, request);

        return result.Map(
            onSuccess: NoContent,
            onFailure: Problem
        );
    }

    [HttpPut("{id:guid}/change-state")]
    public async Task<IActionResult> ChangeState(Guid id)
    {
        var result = await assignmentsService.ChangeState(id);

        return result.Map(
            onSuccess: NoContent,
            onFailure: Problem
        );
    }

    [HttpPut("{id:guid}/change-importance")]
    public async Task<IActionResult> ChangeImportance(Guid id)
    {
        var result = await assignmentsService.ChangeImportance(id);

        return result.Map(
            onSuccess: NoContent,
            onFailure: Problem
        );
    }
}