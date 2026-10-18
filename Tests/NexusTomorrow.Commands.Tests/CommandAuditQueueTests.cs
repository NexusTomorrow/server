using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Ops;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Commands.Tests;

public sealed class CommandAuditQueueTests
{
    private static readonly TimeSpan s_bound = TimeSpan.FromSeconds(30);

    [Fact]
    public async Task AnUnwritableCharacterFromAPlayerNeverBlocksTheAuditQueue()
    {
        await using PostgresTestDatabase? database = await PostgresTestDatabase.CreateAsync();
        if (database is null)
        {
            Assert.Skip($"No PostgreSQL: set {PostgresTestDatabase.ConnectionVariable}.");
        }

        var queue = new WriteBehindQueue(
            database.Contexts, TimeProvider.System, Options.Create(new PersistenceOptions { ConnectionString = database.ConnectionString }),
            NullLogger<WriteBehindQueue>.Instance);
        queue.Start();
        try
        {
            var dispatcher = new CommandDispatcher(
            [
                new CommandDefinition
                {
                    Name = "test secret",
                    Description = "takes a name and a secret",
                    Permission = "test.secret",
                    Parameters = [new("name", "a name"), new("secret", "a secret", Secret: true)],
                    ExecuteAsync = (_, _) => Task.FromResult(new CommandReply(CommandOutcome.Done, "ok")),
                },
            ], queue, TimeProvider.System, NullLogger<CommandDispatcher>.Instance);
            var player = new CommandActor("player", FrontEnd.InGame, new HashSet<string>(), "192.0.2.1");
            var badName = new CommandActor("bad\0name", FrontEnd.InGame, new HashSet<string>(), "192.0.2.\u00007");
            var console = new CommandActor("console", FrontEnd.Cli, new HashSet<string> { PermissionKeys.All }, null);

            await dispatcher.DispatchAsync(player, "test secret al\0ice pw", TestContext.Current.CancellationToken);
            await dispatcher.DispatchAsync(player, "test secret al\ud800ice pw", TestContext.Current.CancellationToken);
            await dispatcher.DispatchAsync(badName, "test secret alice pw", TestContext.Current.CancellationToken);
            await dispatcher.DispatchAsync(console, "test secret al\0ice pw", TestContext.Current.CancellationToken);
            await dispatcher.DispatchAsync(console, "test secret alice pw", TestContext.Current.CancellationToken);

            await queue.DrainAsync(TestContext.Current.CancellationToken).WaitAsync(s_bound, TestContext.Current.CancellationToken);

            await using OpsContext context = database.Contexts.CreateOps();
            List<AuditRow> rows = await context.Audit.OrderBy(a => a.Id).ToListAsync(TestContext.Current.CancellationToken);
            Assert.Equal(["Denied", "Denied", "Denied", "Refused", "Done"], rows.Select(r => r.Outcome));
            Assert.Equal("alice", rows[^1].Target);
        }
        finally
        {
            // A queue stuck on an unwritable row would never drain, so the teardown is bounded too.
            await queue.DisposeAsync().AsTask().WaitAsync(s_bound, TestContext.Current.CancellationToken);
        }
    }
}
