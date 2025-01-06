using DoIt.Api.Persistence.Database;

namespace DoIt.Api.Persistence.Repositories
{
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
}
