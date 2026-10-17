using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Persistence.Ops;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Commands.Tests;

public sealed class CommandDispatcherTests
{
    private static readonly CommandActor s_console = new("console", FrontEnd.Cli, new HashSet<string> { PermissionKeys.All }, null);
    private static readonly CommandActor s_player = new("player", FrontEnd.InGame, new HashSet<string>(), "192.0.2.1");
    private static readonly CommandActor s_gameMaster = new("gm", FrontEnd.InGame, new HashSet<string> { "test.secret", "test.console" }, "192.0.2.2");

    private readonly FakeWriteBehind _db = new();
    private readonly FakeTimeProvider _time = new(new DateTimeOffset(2026, 10, 16, 12, 0, 0, TimeSpan.Zero));
    private readonly List<IReadOnlyList<string>> _ran = [];

    private CommandDispatcher Dispatcher() => new(
    [
        new CommandDefinition
        {
            Name = "test secret",
            Description = "takes a name and a secret",
            Permission = "test.secret",
            Parameters = [new("name", "a name"), new("secret", "a secret", Secret: true)],
            ExecuteAsync = (invocation, _) =>
            {
                _ran.Add(invocation.Arguments);
                return Task.FromResult(new CommandReply(CommandOutcome.Done, "ok"));
            },
        },
        new CommandDefinition
        {
            Name = "test console",
            Description = "console only",
            Permission = "test.console",
            ConsoleOnly = true,
            Parameters = [],
            ExecuteAsync = (_, _) => Task.FromResult(new CommandReply(CommandOutcome.Done, "ok")),
        },
        new CommandDefinition
        {
            Name = "test say",
            Description = "takes a target, an optional level and the rest of the line",
            Permission = null,
            Parameters = [new("target", "who"), new("level", "how loud", Optional: true), new("text", "what to say", Optional: true, Rest: true)],
            ExecuteAsync = (invocation, _) =>
            {
                _ran.Add(invocation.Arguments);
                return Task.FromResult(new CommandReply(CommandOutcome.Done, "ok"));
            },
        },
    ], _db, _time, NullLogger<CommandDispatcher>.Instance);

    [Fact]
    public async Task ACommandRunsWithItsArguments()
    {
        CommandReply reply = await Dispatcher().DispatchAsync(s_console, "test secret alice hunter2", CancellationToken.None);
        Assert.Equal(CommandOutcome.Done, reply.Outcome);
        Assert.Equal(["alice", "hunter2"], _ran.Single());
    }

    [Fact]
    public async Task ACommandWithoutItsPermissionIsDeniedAndAudited()
    {
        CommandReply reply = await Dispatcher().DispatchAsync(s_player, "test secret alice hunter2", CancellationToken.None);
        Assert.Equal(CommandOutcome.Denied, reply.Outcome);
        Assert.Empty(_ran);
        AuditRow audit = _db.Take<AuditWork>().Row;
        Assert.Equal("Denied", audit.Outcome);
        Assert.Equal("player", audit.Actor);
        Assert.Equal("InGame", audit.FrontEnd);
        Assert.Equal("test secret", audit.Action);
        Assert.Equal("192.0.2.1", audit.RemoteAddress);
        Assert.Equal(_time.GetUtcNow(), audit.At);
        Assert.Empty(_db.Queued);
    }

    [Fact]
    public async Task AConsoleOnlyCommandIsDeniedInGameEvenWithItsPermission()
    {
        CommandReply reply = await Dispatcher().DispatchAsync(s_gameMaster, "test console", CancellationToken.None);
        Assert.Equal(CommandOutcome.Denied, reply.Outcome);
        Assert.Equal("Denied", _db.Take<AuditWork>().Row.Outcome);
    }

    [Fact]
    public async Task SecretArgumentsNeverReachTheAudit()
    {
        await Dispatcher().DispatchAsync(s_console, "test secret alice hunter2", CancellationToken.None);
        AuditRow audit = _db.Take<AuditWork>().Row;
        Assert.Equal("Done", audit.Outcome);
        Assert.Equal("alice", audit.Target);
        Assert.Equal("name=alice", audit.Details);
        Assert.Empty(_db.Queued);
    }

    [Fact]
    public async Task ATokenBeyondTheParametersNeverReachesTheAuditOrTheReply()
    {
        // A password with spaces, typed without quotes, spills into extra tokens.
        CommandReply reply = await Dispatcher().DispatchAsync(s_console, "test secret alice correct horse battery", CancellationToken.None);
        Assert.Equal(CommandOutcome.Refused, reply.Outcome);
        Assert.DoesNotContain("horse", reply.Text, StringComparison.Ordinal);
        AuditRow audit = _db.Take<AuditWork>().Row;
        Assert.Equal("Refused", audit.Outcome);
        Assert.DoesNotContain("horse", audit.Details ?? "", StringComparison.Ordinal);
        Assert.DoesNotContain("correct", audit.Details ?? "", StringComparison.Ordinal);
    }

    [Fact]
    public async Task TheWrongNumberOfArgumentsIsRefusedWithTheUsageAndAudited()
    {
        CommandReply reply = await Dispatcher().DispatchAsync(s_console, "test secret alice", CancellationToken.None);
        Assert.Equal(CommandOutcome.Refused, reply.Outcome);
        Assert.Contains("test secret <name> <secret>", reply.Text, StringComparison.Ordinal);
        Assert.Empty(_ran);
        Assert.Equal("Refused", _db.Take<AuditWork>().Row.Outcome);
    }

    [Fact]
    public async Task TheRestParameterTakesTheRemainingTokensAndOptionalOnesMayBeLeftOff()
    {
        CommandDispatcher dispatcher = Dispatcher();
        await dispatcher.DispatchAsync(s_player, "test say bob 3 hello there \"big world\"", CancellationToken.None);
        await dispatcher.DispatchAsync(s_player, "test say bob", CancellationToken.None);
        Assert.Equal(["bob", "3", "hello there big world"], _ran[0]);
        Assert.Equal(["bob"], _ran[1]);
    }

    [Fact]
    public async Task AnUnknownCommandIsRefusedAndNotAudited()
    {
        Assert.Equal(CommandOutcome.Refused, (await Dispatcher().DispatchAsync(s_console, "nothing here", CancellationToken.None)).Outcome);
        Assert.Empty(_db.Queued);
    }

    [Theory]
    [InlineData("test secret alice \"open")]
    [InlineData("   ")]
    public async Task ALineThatDoesNotParseIsRefusedAndNotAudited(string line)
    {
        Assert.Equal(CommandOutcome.Refused, (await Dispatcher().DispatchAsync(s_console, line, CancellationToken.None)).Outcome);
        Assert.Empty(_db.Queued);
    }

    [Fact]
    public async Task HelpListsOnlyWhatTheActorHoldsAndIsNotAudited()
    {
        CommandReply reply = await Dispatcher().DispatchAsync(new CommandActor("gm", FrontEnd.Cli, new HashSet<string> { "test.console" }, null), "help", CancellationToken.None);
        Assert.Contains("test console", reply.Text, StringComparison.Ordinal);
        Assert.Contains("test say <target> [level] [text...]", reply.Text, StringComparison.Ordinal);
        Assert.DoesNotContain("test secret", reply.Text, StringComparison.Ordinal);
        Assert.Empty(_db.Queued);
    }

    [Fact]
    public async Task AThrowingCommandIsReportedAsFailedAndAudited()
    {
        var dispatcher = new CommandDispatcher(
        [
            new CommandDefinition
            {
                Name = "test throws",
                Description = "throws",
                Permission = null,
                Parameters = [],
                ExecuteAsync = (_, _) => throw new InvalidOperationException("thrown on purpose"),
            },
        ], _db, _time, NullLogger<CommandDispatcher>.Instance);
        CommandReply reply = await dispatcher.DispatchAsync(s_console, "test throws", CancellationToken.None);
        Assert.Equal(CommandOutcome.Failed, reply.Outcome);
        Assert.DoesNotContain("thrown on purpose", reply.Text, StringComparison.Ordinal);
        Assert.Equal("Failed", _db.Take<AuditWork>().Row.Outcome);
    }

    [Fact]
    public async Task AnActorWithoutThePermissionIsDeniedBeforeItsArgumentsAreChecked()
    {
        CommandReply reply = await Dispatcher().DispatchAsync(s_player, "test secret alice", CancellationToken.None);
        Assert.Equal(CommandOutcome.Denied, reply.Outcome);
        Assert.DoesNotContain("<name>", reply.Text, StringComparison.Ordinal);
        Assert.DoesNotContain("Usage", reply.Text, StringComparison.Ordinal);
        Assert.Equal("Denied", _db.Take<AuditWork>().Row.Outcome);
    }

    [Fact]
    public async Task AConsoleOnlyCommandIsDeniedBeforeItsArgumentsAreChecked()
    {
        CommandReply reply = await Dispatcher().DispatchAsync(s_gameMaster, "test console surplus", CancellationToken.None);
        Assert.Equal(CommandOutcome.Denied, reply.Outcome);
        Assert.Equal("Denied", _db.Take<AuditWork>().Row.Outcome);
    }

    // Built in code: a lone surrogate does not survive being a theory's data.
    private static readonly string[] s_unstorable = ["\0", "\n", "\ud800", "\udc00"];

    [Fact]
    public async Task AControlCharacterOrLoneSurrogateIsDeniedWithoutReachingTheAudit()
    {
        foreach (string bad in s_unstorable)
        {
            CommandReply reply = await Dispatcher().DispatchAsync(s_player, $"test secret al{bad}ice pw", CancellationToken.None);
            Assert.Equal(CommandOutcome.Denied, reply.Outcome);
            AuditRow audit = _db.Take<AuditWork>().Row;
            Assert.Null(audit.Target);
            Assert.Null(audit.Details);
        }
    }

    [Fact]
    public async Task AControlCharacterOrLoneSurrogateInAPermittedLineIsRefusedAndNeverRuns()
    {
        foreach (string bad in s_unstorable)
        {
            CommandReply reply = await Dispatcher().DispatchAsync(s_console, $"test secret al{bad}ice pw", CancellationToken.None);
            Assert.Equal(CommandOutcome.Refused, reply.Outcome);
            Assert.Empty(_ran);
            AuditRow audit = _db.Take<AuditWork>().Row;
            Assert.Equal("Refused", audit.Outcome);
            Assert.Null(audit.Target);
        }
    }

    [Fact]
    public async Task AValidSurrogatePairIsKept()
    {
        await Dispatcher().DispatchAsync(s_console, "test secret al😀ice pw", CancellationToken.None);
        Assert.Equal("al😀ice", _db.Take<AuditWork>().Row.Target);
    }

    [Fact]
    public async Task AControlCharacterInTheActorNameOrAddressIsScrubbedFromTheAudit()
    {
        var actor = new CommandActor("ba\0d\ud800name", FrontEnd.InGame, new HashSet<string>(), "192.0.2.\u00007");
        await Dispatcher().DispatchAsync(actor, "test secret alice pw", CancellationToken.None);
        AuditRow audit = _db.Take<AuditWork>().Row;
        Assert.Equal("ba?d?name", audit.Actor);
        Assert.Equal("192.0.2.?7", audit.RemoteAddress);
    }

    [Fact]
    public async Task AnOverlongArgumentIsClippedToTheAuditColumnsSoTheRowCanBeWritten()
    {
        await Dispatcher().DispatchAsync(s_console, $"test secret {new string('a', 3000)} pw", CancellationToken.None);
        AuditRow audit = _db.Take<AuditWork>().Row;
        Assert.Equal(200, audit.Target!.Length);
        Assert.Equal(2000, audit.Details!.Length);
    }
}
