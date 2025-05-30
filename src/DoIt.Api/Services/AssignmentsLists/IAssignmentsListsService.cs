using DoIt.Api.Controllers.AssignmentsLists;
using DoIt.Api.Shared;

namespace DoIt.Api.Services.AssignmentsLists;

public interface IAssignmentsListsService
{
    Task<List<AssignmentsListDto>> GetAll();
    Task<Result<AssignmentsListDto>> Create(CreateAssignmentsListRequest request);
    Task<Result<AssignmentsListDto>> GetById(Guid id);
    Task<Result> AttachAssignment(Guid id, Guid assignmentId);
    Task<Result> DetachAssignment(Guid id, Guid  assignmentId);
}