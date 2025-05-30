using DoIt.Api.Controllers.AssignmentsLists;
using DoIt.Api.Domain.AssignmentsLists;
using DoIt.Api.Persistence.Repositories.AssignmentsLists;
using DoIt.Api.Shared;

namespace DoIt.Api.Services.AssignmentsLists;

public class AssignmentsListsService(IAssignmentsListsRepository repository)
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

        if (createAssignmentsListResult.IsFailure)
            return createAssignmentsListResult.Error!;

        return createAssignmentsListResult.Value!.ToDto();
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
}