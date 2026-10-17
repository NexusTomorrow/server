using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Commands.Tests;

public sealed class ServerCommandsTests : IAsyncLifetime
{
    private static readonly CommandActor s_console = new("console", FrontEnd.Cli, new HashSet<string> { PermissionKeys.All }, null);

    private readonly FakeWriteBehind _db = new();
    private readonly AccountOwner _accounts;
    private readonly CommandDispatcher _dispatcher;

    public ServerCommandsTests()
    {
        var time = new FakeTimeProvider(new DateTimeOffset(2026, 10, 16, 12, 0, 0, TimeSpan.Zero));
        _accounts = new AccountOwner(time, Options.Create(new AccountOptions()), NullLogger<AccountOwner>.Instance, _db);
        _dispatcher = new CommandDispatcher(ServerCommands.Create(_accounts, [_accounts], _db), _db, time, NullLogger<CommandDispatcher>.Instance);
    }

    public ValueTask InitializeAsync() => ValueTask.CompletedTask;

    public async ValueTask DisposeAsync() => await _accounts.StopAsync().WaitAsync(TimeSpan.FromSeconds(10));

    [Fact]
    public async Task ServerStatusReportsOneFactPerLine()
    {
        CommandReply reply = await _dispatcher.DispatchAsync(s_console, "server status", CancellationToken.None);
        Assert.Equal(CommandOutcome.Done, reply.Outcome);
        Assert.Equal(
            ["accounts online: 0", "account queue: 0", "persistence backlog: 0", "database: reachable"],
            reply.Text.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));
    }

    [Fact]
    public async Task ServerStatusSaysWhenTheDatabaseIsUnreachable()
    {
        _db.IsAvailable = false;
        CommandReply reply = await _dispatcher.DispatchAsync(s_console, "server status", CancellationToken.None);
        Assert.Contains("database: unreachable", reply.Text, StringComparison.Ordinal);
    }
}
