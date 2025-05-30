using DoIt.Api.Domain.Shared;
using DoIt.Api.Domain.Assignments;
using DoIt.Api.Shared;

namespace DoIt.Api.Domain.AssignmentsLists;

public class AssignmentsList
    : Entity<AssignmentsListId>
{
    public Name Name { get; }
    public DateTime CreatedAt { get; }
    public List<Assignment> Assignments { get; }
    
    private AssignmentsList(
        AssignmentsListId assignmentsListId,
        Name name,
        DateTime createdAt,
        List<Assignment> assignments
    )
        : base(assignmentsListId)
    {
        Name = name;
        CreatedAt = createdAt;
        Assignments = assignments;
    }

    public static Result<AssignmentsList> Create(
        AssignmentsListId assignmentsListId,
        Name name,
        DateTime createdAt,
        List<Assignment>? assignments = null
    )
    {
        return new AssignmentsList(
            assignmentsListId,
            name,
            createdAt,
            assignments ?? []
        );
    }
}