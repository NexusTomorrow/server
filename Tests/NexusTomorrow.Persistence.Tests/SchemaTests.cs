using Microsoft.EntityFrameworkCore;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Account;
using NexusTomorrow.Persistence.Realm;
using NexusTomorrow.Testing;
using Npgsql;

namespace NexusTomorrow.Persistence.Tests;

public sealed class SchemaTests
{
    private static Task<PostgresTestDatabase> DatabaseAsync(bool migrate = true) => Databases.CreateAsync(migrate);

    [Fact]
    public async Task MigrationsApplyToAnEmptyDatabaseAndLeaveNoPendingChanges()
    {
        await using PostgresTestDatabase database = await DatabaseAsync(migrate: false);
        await database.Contexts.MigrateAsync(TestContext.Current.CancellationToken);

        await using AccountContext account = database.Contexts.CreateAccount();
        await using RealmContext realm = database.Contexts.CreateRealm();
        await using Ops.OpsContext ops = database.Contexts.CreateOps();
        Assert.False(account.Database.HasPendingModelChanges());
        Assert.False(realm.Database.HasPendingModelChanges());
        Assert.False(ops.Database.HasPendingModelChanges());
        Assert.Empty(await account.Database.GetPendingMigrationsAsync(TestContext.Current.CancellationToken));
        Assert.Empty(await realm.Database.GetPendingMigrationsAsync(TestContext.Current.CancellationToken));
        Assert.Empty(await ops.Database.GetPendingMigrationsAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task AnEmailIsUniqueWithoutRegardToCase()
    {
        await using PostgresTestDatabase database = await DatabaseAsync();
        await using AccountContext context = database.Contexts.CreateAccount();
        context.Accounts.Add(new AccountRow { Email = "Alice@Example.com", Salt = [1], Verifier = [2], CreatedAt = DateTimeOffset.UnixEpoch });
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        await using AccountContext second = database.Contexts.CreateAccount();
        second.Accounts.Add(new AccountRow { Email = "alice@example.com", Salt = [1], Verifier = [2], CreatedAt = DateTimeOffset.UnixEpoch });
        DbUpdateException failure = await Assert.ThrowsAsync<DbUpdateException>(() => second.SaveChangesAsync(TestContext.Current.CancellationToken));
        Assert.Equal(PostgresErrorCodes.UniqueViolation, Assert.IsType<PostgresException>(failure.InnerException).SqlState);

        await using AccountContext reader = database.Contexts.CreateAccount();
        Assert.NotNull(await reader.Accounts.SingleOrDefaultAsync(a => a.Email == "ALICE@EXAMPLE.COM", TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task ALiveCharacterNameIsUniqueWithoutRegardToCaseAndADeletedOneFreesIt()
    {
        await using PostgresTestDatabase database = await DatabaseAsync();
        await using (RealmContext context = database.Contexts.CreateRealm())
        {
            context.Characters.Add(Character(1, "Rebaser Head"));
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (RealmContext context = database.Contexts.CreateRealm())
        {
            context.Characters.Add(Character(2, "rebaser head"));
            DbUpdateException failure = await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync(TestContext.Current.CancellationToken));
            Assert.Equal(PostgresErrorCodes.UniqueViolation, Assert.IsType<PostgresException>(failure.InnerException).SqlState);
        }

        await using (RealmContext context = database.Contexts.CreateRealm())
        {
            await context.Characters.Where(c => c.Id == 1).ExecuteUpdateAsync(s => s.SetProperty(c => c.DeletedAt, DateTimeOffset.UnixEpoch), TestContext.Current.CancellationToken);
            context.Characters.Add(Character(2, "rebaser head"));
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }
    }

    [Theory]
    [InlineData("realm", "character", "id")]
    [InlineData("realm", "character", "creation_id")]
    [InlineData("realm", "character", "faction_id")]
    [InlineData("realm", "character_customization", "value")]
    public async Task UnsignedColumnsAreBigint(string schema, string table, string column)
    {
        await using PostgresTestDatabase database = await DatabaseAsync();
        await using var connection = new NpgsqlConnection(database.ConnectionString);
        await connection.OpenAsync(TestContext.Current.CancellationToken);
        await using var command = new NpgsqlCommand(
            "SELECT data_type FROM information_schema.columns WHERE table_schema = @s AND table_name = @t AND column_name = @c", connection);
        command.Parameters.AddWithValue("s", schema);
        command.Parameters.AddWithValue("t", table);
        command.Parameters.AddWithValue("c", column);
        Assert.Equal("bigint", await command.ExecuteScalarAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task CharacterIdsComeInBlocksOfAThousand()
    {
        await using PostgresTestDatabase database = await DatabaseAsync();
        await using RealmContext context = database.Contexts.CreateRealm();
        long first = await context.Database.SqlQueryRaw<long>($"SELECT nextval('realm.{RealmContext.CharacterIdSequence}') AS \"Value\"").SingleAsync(TestContext.Current.CancellationToken);
        long second = await context.Database.SqlQueryRaw<long>($"SELECT nextval('realm.{RealmContext.CharacterIdSequence}') AS \"Value\"").SingleAsync(TestContext.Current.CancellationToken);
        Assert.Equal(1, first);
        Assert.Equal(RealmContext.CharacterIdBlock, second - first);
    }

    private static CharacterRow Character(ulong id, string name) => new()
    {
        Id = id,
        AccountId = 1,
        Name = name,
        CreationId = 1,
        Race = 1,
        Sex = 0,
        Class = 1,
        FactionId = 166,
        Path = 0,
        Level = 1,
        CreatedAt = DateTimeOffset.UnixEpoch,
    };
}
