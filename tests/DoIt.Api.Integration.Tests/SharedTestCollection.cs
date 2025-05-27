using DoIt.Api.Persistence.Repositories.TaskLists;
using DoIt.Api.Persistence.Repositories.Tasks;

namespace DoIt.Api.Integration.Tests;

[CollectionDefinition("Tasks controller tests")]
public class SharedTestCollection
    : ICollectionFixture<DoItApiFactory>;
