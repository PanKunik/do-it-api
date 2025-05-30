using DoIt.Api.Domain.AssignmentsLists;
using DoIt.Api.Shared;

namespace DoIt.Api.Persistence.Repositories.AssignmentsLists;

public interface IAssignmentsListsRepository
{
    Task<List<AssignmentsList>> GetAll();
    Task<Result<AssignmentsList>> Create(AssignmentsList assignmentsList);
    Task<Result> Delete(AssignmentsListId assignmentsListId);
    Task<Result<AssignmentsList>> GetById(AssignmentsListId assignmentsListId);
}