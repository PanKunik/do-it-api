using DoIt.Api.Domain.Assignments;
using DoIt.Api.Domain.AssignmentsLists;
using DoIt.Api.Persistence.Repositories.AssignmentsLists;

namespace DoIt.Api.TestUtils.Builders;

public class AssignmentsListBuilder
{
    private AssignmentsListId Id { get; set; }
    private Name Name { get; set; }
    private DateTime CreatedAt { get; set; }
    private List<Assignment> Assignments { get; set; }

    private AssignmentsListBuilder(
        AssignmentsListId id,
        Name name,
        DateTime createdAt,
        List<Assignment> assignments
    )
    {
        Id = id;
        Name = name;
        CreatedAt = createdAt;
        Assignments = assignments;
    }
    
    public static AssignmentsListBuilder Default(int no = 1)
    {
        return new AssignmentsListBuilder(
            AssignmentsListId.CreateFrom(new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, (byte)no)).Value!,
            Name.CreateFrom($"Name {no}").Value!,
            new DateTime(2025, 1, 1).AddDays(no),
            new  List<Assignment>()
        );
    }

    public AssignmentsListBuilder WithName(Name name)
    {
        Name = name;
        return this;
    }

    public AssignmentsListBuilder WithAssignment(Assignment assignment)
    {
        Assignments.Add(assignment);
        return this;
    }

    public AssignmentsList Build()
    {
        return AssignmentsList.Create(
            Id,
            Name,
            CreatedAt,
            Assignments
        ).Value!;
    }

    public async Task SaveInRepository(IAssignmentsListsRepository assignmentsListsRepository)
        => await assignmentsListsRepository.Create(Build());
}