using DoIt.Api.Domain.Assignments;
using DoIt.Api.Shared;

namespace DoIt.Api.Persistence.Repositories.Assignments;

public interface IAssignmentsRepository
{
    Task<List<Assignment>> GetAll();
    Task<Result<Assignment>> GetById(AssignmentId assignmentId);
    Task<Result<Assignment>> Create(Assignment assignment);
    Task<Result> Delete(AssignmentId assignmentId);
    Task<Result> Update(Assignment assignment);
}