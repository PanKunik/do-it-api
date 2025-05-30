using DoIt.Api.Controllers.Assignments;
using DoIt.Api.Domain.AssignmentsLists;
using DoIt.Api.Domain.Assignments;
using DoIt.Api.Persistence.Repositories.AssignmentsLists;
using DoIt.Api.Persistence.Repositories.Assignments;
using DoIt.Api.Shared;

namespace DoIt.Api.Services.Assignments;

public class AssignmentsService(
    IAssignmentsRepository assignmentsRepository,
    IAssignmentsListsRepository assignmentsListsRepository
)
    : IAssignmentsService
{
    public async Task<List<AssignmentDto>> GetAll()
    {
        var result = await assignmentsRepository.GetAll();
        return result
            .Select(r => r.ToDto())
            .ToList();
    }

    public async Task<Result<AssignmentDto>> GetById(Guid id)
    {
        var assignmentIdResult = AssignmentId.CreateFrom(id);

        if (assignmentIdResult.IsFailure)
            return assignmentIdResult.Error!;

        var result = await assignmentsRepository.GetById(assignmentIdResult.Value!);

        return result.Map<Result<AssignmentDto>>(
            onSuccess: value => value.ToDto(),
            onFailure: error => error
        );
    }

    public async Task<Result<AssignmentDto>> Create(CreateAssignmentRequest request)
    {
        var assignmentTitleResult = Title.CreateFrom(request.Title);

        if (assignmentTitleResult.IsFailure)
            return assignmentTitleResult.Error!;

        Result<AssignmentsListId>? assignmentsListIdResult = null;

        if (request.AssignmentsListId is not null)
        {
            assignmentsListIdResult = AssignmentsListId.CreateFrom(request.AssignmentsListId.Value);
            
            if (assignmentsListIdResult.IsFailure)
                return assignmentsListIdResult.Error!;
            
            var existingAssignmentsListResult = await assignmentsListsRepository.GetById(assignmentsListIdResult.Value!);
            
            if (existingAssignmentsListResult.IsFailure)
                return existingAssignmentsListResult.Error!;
        }
        
        var assignmentResult = Assignment.Create(
            AssignmentId.CreateUnique(),
            assignmentTitleResult.Value!,
            DateTime.UtcNow,
            false,
            request.IsImportant ?? false,
            assignmentsListIdResult?.Value
        );

        if (assignmentResult.IsFailure)
            return assignmentResult.Error!;

        var createAssignmentResult = await assignmentsRepository.Create(assignmentResult.Value!);

        if (createAssignmentResult.IsFailure)
            return createAssignmentResult.Error!;

        return createAssignmentResult.Value!.ToDto();
    }

    public async Task<Result> Delete(Guid id)
    {
        var assignmentIdResult = AssignmentId.CreateFrom(id);

        if (assignmentIdResult.IsFailure)
            return assignmentIdResult.Error!;

        return await assignmentsRepository.Delete(assignmentIdResult.Value!);
    }

    public async Task<Result> Update(
        Guid id,
        UpdateAssignmentRequest request
    )
    {
        var assignmentIdResult = AssignmentId.CreateFrom(id);

        if (assignmentIdResult.IsFailure)
            return assignmentIdResult.Error!;

        var titleResult = Title.CreateFrom(request.Title);

        if (titleResult.IsFailure)
            return titleResult.Error!;

        var assignmentToUpdateResult = await assignmentsRepository.GetById(assignmentIdResult.Value!);

        if (assignmentToUpdateResult.IsFailure)
            return assignmentToUpdateResult.Error!;

        var assignmentToUpdate = assignmentToUpdateResult.Value!;
        assignmentToUpdate.UpdateTitle(titleResult.Value!);

        return await assignmentsRepository.Update(assignmentToUpdate);
    }

    public async Task<Result> ChangeState(Guid id)
    {
        var assignmentIdResult = AssignmentId.CreateFrom(id);
        
        if (assignmentIdResult.IsFailure)
            return assignmentIdResult.Error!;

        var assignmentToDoResult = await assignmentsRepository.GetById(assignmentIdResult.Value!);

        if (assignmentToDoResult.IsFailure)
            return assignmentToDoResult.Error!;

        var assignmentToDo = assignmentToDoResult.Value!;
        assignmentToDo.ChangeState();

        await assignmentsRepository.Update(assignmentToDo);
        
        return Result.Success();
    }

    public async Task<Result> ChangeImportance(Guid id)
    {
        var assignmentIdResult = AssignmentId.CreateFrom(id);
        
        if (assignmentIdResult.IsFailure)
            return assignmentIdResult.Error!;

        var assignmentToChangeResult = await assignmentsRepository.GetById(assignmentIdResult.Value!);

        if (assignmentToChangeResult.IsFailure)
            return assignmentToChangeResult.Error!;

        var assignmentToChange = assignmentToChangeResult.Value!;
        assignmentToChange.ChangeImportance();

        await assignmentsRepository.Update(assignmentToChange);
        
        return Result.Success();
    }
}