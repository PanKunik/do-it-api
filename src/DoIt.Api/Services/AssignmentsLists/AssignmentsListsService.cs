using DoIt.Api.Controllers.AssignmentsLists;
using DoIt.Api.Domain.Assignments;
using DoIt.Api.Domain.AssignmentsLists;
using DoIt.Api.Persistence.Repositories.Assignments;
using DoIt.Api.Persistence.Repositories.AssignmentsLists;
using DoIt.Api.Shared;

namespace DoIt.Api.Services.AssignmentsLists;

public class AssignmentsListsService(
    IAssignmentsListsRepository repository,
    IAssignmentsRepository assignmentsRepository
)
    : IAssignmentsListsService
{
    public async Task<Result<AssignmentsListDto>> Create(CreateAssignmentsListRequest request)
    {
        var assignmentsListNameResult = Name.CreateFrom(request.Name);
        
        if (assignmentsListNameResult.IsFailure)
            return assignmentsListNameResult.Error!;

        var assignmentsListResult = AssignmentsList.Create(
            AssignmentsListId.CreateUnique(),
            assignmentsListNameResult.Value!,
            DateTime.UtcNow
        );
        
        if (assignmentsListResult.IsFailure)
            return assignmentsListResult.Error!;

        var createAssignmentsListResult = await repository.Create(assignmentsListResult.Value!);
        return createAssignmentsListResult.Map<Result<AssignmentsListDto>>(
            onSuccess: r => r.ToDto(),
            onFailure: r => r
        );
    }

    public async Task<Result> Delete(Guid id)
    {
        var assignmentsListIdResult = AssignmentsListId.CreateFrom(id);

        if (assignmentsListIdResult.IsFailure)
            return assignmentsListIdResult.Error!;
        
        var assignmentsListResult = await repository.GetById(assignmentsListIdResult.Value!);
        
        if (assignmentsListResult.IsFailure)
            return assignmentsListResult.Error!;

        var assignments = assignmentsListResult.Value!.Assignments;
        foreach (var assignment in assignments)
        {
            assignment.DetachFromList();
            var assignmentUpdateResult = await assignmentsRepository.Update(assignment);
            
            if (assignmentUpdateResult.IsFailure)
                return assignmentUpdateResult.Error!;
        }

        return await repository.Delete(assignmentsListIdResult.Value!);
    }

    public async Task<List<AssignmentsListDto>> GetAll()
    {
        var result = await repository.GetAll();
        return result
            .Select(r => r.ToDto())
            .ToList();
    }

    public async Task<Result<AssignmentsListDto>> GetById(Guid id)
    {
        var assignmentsListIdResult = AssignmentsListId.CreateFrom(id);

        if (assignmentsListIdResult.IsFailure)
            return assignmentsListIdResult.Error!;

        var result = await repository.GetById(assignmentsListIdResult.Value!);

        return result.Map<Result<AssignmentsListDto>>(
            onSuccess: value => value.ToDto(),
            onFailure: error => error
        );
    }

    public async Task<Result> AttachAssignment(Guid id, Guid assignmentId)
    {
        var assignmentIdResult = AssignmentId.CreateFrom(assignmentId);
        if (assignmentIdResult.IsFailure)
            return assignmentIdResult.Error!;

        var assignmentsListIdResult = AssignmentsListId.CreateFrom(id);
        if (assignmentsListIdResult.IsFailure)
            return assignmentsListIdResult.Error!;
        
        var assignmentResult = await assignmentsRepository.GetById(assignmentIdResult.Value!);
        if (assignmentResult.IsFailure)
            return assignmentResult.Error!;

        var assignmentsListResult = await repository.GetById(assignmentsListIdResult.Value!);
        if (assignmentsListResult.IsFailure)
            return assignmentsListResult.Error!;
        
        assignmentsListResult.Value!.AttachAssignment(assignmentResult.Value!);
        return await assignmentsRepository.Update(assignmentResult.Value!);
    }
    
    public async Task<Result> DetachAssignment(Guid id, Guid assignmentId)
    {
        var assignmentIdResult = AssignmentId.CreateFrom(assignmentId);
        if (assignmentIdResult.IsFailure)
            return assignmentIdResult.Error!;

        var assignmentsListIdResult = AssignmentsListId.CreateFrom(id);
        if (assignmentsListIdResult.IsFailure)
            return assignmentsListIdResult.Error!;
        
        var assignmentResult = await assignmentsRepository.GetById(assignmentIdResult.Value!);
        if (assignmentResult.IsFailure)
            return assignmentResult.Error!;

        var assignmentsListResult = await repository.GetById(assignmentsListIdResult.Value!);
        if (assignmentsListResult.IsFailure)
            return assignmentsListResult.Error!;
        
        var detachResult = assignmentsListResult.Value!.DetachAssignment(assignmentResult.Value!);
        if (detachResult.IsFailure)
            return detachResult.Error!;
        
        return await assignmentsRepository.Update(assignmentResult.Value!);
    }
}