using DoIt.Api.Controllers.Assignments;
using DoIt.Api.Shared;

namespace DoIt.Api.Services.Assignments;

public interface IAssignmentsService
{
    Task<List<AssignmentDto>> GetAll();
    Task<Result<AssignmentDto>> GetById(Guid id);
    Task<Result<AssignmentDto>> Create(CreateAssignmentRequest request);
    Task<Result> Delete(Guid id);
    Task<Result> Update(Guid id, UpdateAssignmentRequest request);
    Task<Result> ChangeState(Guid id);
    Task<Result> ChangeImportance(Guid id);
}