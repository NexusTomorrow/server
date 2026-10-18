using NexusTomorrow.Testing;

namespace NexusTomorrow.Persistence.Tests;

internal static partial class Databases
{
    public static async Task<PostgresTestDatabase> CreateAsync(bool migrate = true)
    {
        PostgresTestDatabase? database = await PostgresTestDatabase.CreateAsync(migrate);
        if (database is null)
            Assert.Skip($"No PostgreSQL: set {PostgresTestDatabase.ConnectionVariable}.");
        return database;
    }
}
