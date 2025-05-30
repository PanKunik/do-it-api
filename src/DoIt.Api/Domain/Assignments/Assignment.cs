using DoIt.Api.Domain.Shared;
using DoIt.Api.Domain.AssignmentsLists;
using DoIt.Api.Shared;

namespace DoIt.Api.Domain.Assignments;

public class Assignment
    : Entity<AssignmentId>
{
    public Title Title { get; private set; }
    public DateTime CreatedAt { get; }
    public bool IsDone { get; private set; }
    public bool IsImportant { get; private set; }
    public AssignmentsListId? AssignmentsListId { get; private set; }

    private Assignment(
        AssignmentId id,
        Title title,
        DateTime createdAt,
        bool isDone,
        bool isImportant,
        AssignmentsListId? assignmentsListId
    )
        : base(id)
    {
        Title = title;
        CreatedAt = createdAt;
        IsDone = isDone;
        IsImportant = isImportant;
        AssignmentsListId = assignmentsListId;
    }

    public static Result<Assignment> Create(
        AssignmentId id,
        Title title,
        DateTime createdAt,
        bool isDone,
        bool isImportant,
        AssignmentsListId? assignmentsListId
    )
    {
        return new Assignment(
            id,
            title,
            createdAt,
            isDone,
            isImportant,
            assignmentsListId
        );
    }

    public void AttachToList(AssignmentsListId assignmentsListId)
        => AssignmentsListId = assignmentsListId;
    
    public void DetachFromList()
        => AssignmentsListId = null;
    
    public void UpdateTitle(Title title)
        => Title = title;

    public void ChangeState()
        => IsDone = !IsDone;

    public void ChangeImportance()
        => IsImportant = !IsImportant;
}