using NexusTomorrow.Testing;

namespace NexusTomorrow.Server.Tests;

internal static class Databases
{
    public static async Task<PostgresTestDatabase> CreateAsync(bool migrate = true)
    {
        PostgresTestDatabase? database = await PostgresTestDatabase.CreateAsync(migrate);
        if (database is null)
            Assert.Skip($"No PostgreSQL: set {PostgresTestDatabase.ConnectionVariable}.");
        return database;
    }
}
