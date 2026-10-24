using NexusTomorrow.Persistence;
using Npgsql;

namespace NexusTomorrow.Testing;

// A fresh database per test on the server NEXUSTOMORROW_TEST_PG names, migrated, and dropped on dispose. With
// NEXUSTOMORROW_TEST_PG_REQUIRED=1 (CI sets it), a missing variable is a failure, never a skip.
public sealed class PostgresTestDatabase : IAsyncDisposable
{
    public const string ConnectionVariable = "NEXUSTOMORROW_TEST_PG";
    public const string RequiredVariable = "NEXUSTOMORROW_TEST_PG_REQUIRED";

    private readonly string _adminConnectionString;
    private readonly string _name;

    private PostgresTestDatabase(string adminConnectionString, string name, string connectionString)
    {
        _adminConnectionString = adminConnectionString;
        _name = name;
        ConnectionString = connectionString;
        Contexts = PersistenceContexts.Create(connectionString);
    }

    public string ConnectionString { get; }

    public PersistenceContexts Contexts { get; }

    public static async Task<PostgresTestDatabase?> CreateAsync(bool migrate = true)
    {
        string? admin = Environment.GetEnvironmentVariable(ConnectionVariable);
        if (string.IsNullOrEmpty(admin))
        {
            if (Environment.GetEnvironmentVariable(RequiredVariable) == "1")
                throw new InvalidOperationException($"{RequiredVariable} is set but {ConnectionVariable} is not.");
            return null;
        }

        string name = $"nt_test_{Guid.NewGuid():N}";
        await using (var connection = new NpgsqlConnection(admin))
        {
            await connection.OpenAsync();
            await using var command = new NpgsqlCommand($"CREATE DATABASE \"{name}\"", connection);
            await command.ExecuteNonQueryAsync();
        }

        var builder = new NpgsqlConnectionStringBuilder(admin) { Database = name };
        var database = new PostgresTestDatabase(admin, name, builder.ConnectionString);
        if (migrate)
            await database.Contexts.MigrateAsync(CancellationToken.None);
        return database;
    }

    // FORCE ends every other backend on the database first, an autovacuum worker among them; that worker carries no role, so the
    // server's role needs pg_signal_backend to end it (test.yml grants it), or the drop fails whenever autovacuum is at work there.
    public async ValueTask DisposeAsync()
    {
        NpgsqlConnection.ClearAllPools();
        await using var connection = new NpgsqlConnection(_adminConnectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand($"DROP DATABASE IF EXISTS \"{_name}\" WITH (FORCE)", connection);
        await command.ExecuteNonQueryAsync();
    }
}
