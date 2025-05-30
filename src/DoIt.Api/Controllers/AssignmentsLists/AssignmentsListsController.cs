using DoIt.Api.Controllers._Common;
using DoIt.Api.Services.AssignmentsLists;
using Microsoft.AspNetCore.Mvc;

namespace DoIt.Api.Controllers.AssignmentsLists;

[Route("api/assignments-lists")]
public class AssignmentsListsController(IAssignmentsListsService assignmentsListsService)
    : ApiController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var assignmentsLists = await assignmentsListsService.GetAll();
        return Ok(assignmentsLists);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateAssignmentsListRequest request)
    {
        var result = await assignmentsListsService.Create(request);

        return result.Map(
            onSuccess: (createdAssignmentsList)
                => CreatedAtAction(
                    actionName: nameof(GetById),
                    routeValues: new { id = createdAssignmentsList.Id },
                    value: null
                ),
                onFailure: Problem
        );
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await assignmentsListsService.GetById(id);
        
        return result.Map(
            onSuccess: Ok,
            onFailure: Problem
        );
    }
}