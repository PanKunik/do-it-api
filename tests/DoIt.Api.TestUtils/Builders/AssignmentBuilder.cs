using DoIt.Api.Domain.AssignmentsLists;
using DoIt.Api.Domain.Assignments;
using DoIt.Api.Persistence.Repositories.Assignments;

namespace DoIt.Api.TestUtils.Builders;

public class AssignmentBuilder
{
    private AssignmentId Id { get; set; }
    private Title Title { get; set; }
    private DateTime CreatedAt  { get; set; }
    private bool IsDone  { get; set; }
    private bool IsImportant  { get; set; }
    private AssignmentsListId? AssignmentsListId { get; set; }

    private AssignmentBuilder(
        AssignmentId id,
        Title title,
        DateTime createdAt,
        bool? isDone = null,
        bool? isImportant = null,
        AssignmentsListId? assignmentsListId = null
    )
    {
        Id = id;
        Title = title;
        CreatedAt = createdAt;
        IsDone = isDone ?? false;
        IsImportant = isImportant ?? false;
        AssignmentsListId = assignmentsListId;
    }
    
    public static AssignmentBuilder Default(int no = 1)
    {
        return new AssignmentBuilder(
            AssignmentId.CreateFrom(new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, (byte)no)).Value!,
            Title.CreateFrom($"Title {no}").Value!,
            new DateTime(2025, 1, 1).AddDays(no),
            false,
            false
        );
    }

    public AssignmentBuilder WithId(Guid id)
    {
        Id = AssignmentId.CreateFrom(id).Value!;
        return this;
    }

    public AssignmentBuilder WithIsDone(bool isDone)
    {
        IsDone = isDone;
        return this;
    }

    public AssignmentBuilder WithIsImportant(bool isImportant)
    {
        IsImportant = isImportant;
        return this;
    }

    public AssignmentBuilder WithAssignmentsListId(Guid assignmentsListId)
    {
        AssignmentsListId = AssignmentsListId.CreateFrom(assignmentsListId).Value!;
        return this;
    }

    public Assignment Build()
    {
        return Assignment.Create(
            Id,
            Title,
            CreatedAt,
            IsDone,
            IsImportant,
            AssignmentsListId
        ).Value!;
    }

    public async Task SaveInRepository(IAssignmentsRepository assignmentsRepository)
        => await assignmentsRepository.Create(Build());
}