using Dapper;
using DoIt.Api.Domain.Tasks;
using DoIt.Api.Persistence.Database;
using System.Data;
using Task = DoIt.Api.Domain.Tasks.Task;

namespace DoIt.Api.Persistence.Repositories;

public class TasksRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public TasksRepository(
        IDbConnectionFactory dbConnectionFactory    
    )
    {
        _dbConnectionFactory = dbConnectionFactory
            ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
    }
}
