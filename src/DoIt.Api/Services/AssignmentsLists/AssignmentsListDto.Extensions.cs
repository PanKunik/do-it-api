using DoIt.Api.Domain.AssignmentsLists;
using DoIt.Api.Services.Assignments;

namespace DoIt.Api.Services.AssignmentsLists;

public static class Extensions
{
    public static AssignmentsListDto ToDto(this AssignmentsList assignmentsList)
        => new(
                assignmentsList.Id.Value,
                assignmentsList.Name.Value,
                assignmentsList.CreatedAt,
                assignmentsList.Assignments.Select(t => t.ToDto()).ToList()
            );
}