﻿using DbUp;
using System.Reflection;

namespace DoIt.Api.Persistence.Database;

public static class DbInitializer
{
    public static void Initialize(string connectionString)
    {
        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        var upgrade = DeployChanges.To.PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .WithTransaction()
            .LogToConsole()
            .Build();

        var result = upgrade.PerformUpgrade();

        if (!result.Successful)
            throw new InvalidOperationException("Failed to upgrade the database.");
    }
}