using System.Numerics;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Persistence.Ops;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Commands.Tests;

public sealed class CharacterCommandsTests
{
    private static readonly CommandActor s_console = new("console", FrontEnd.Cli, new HashSet<string> { PermissionKeys.All }, null);
    private static readonly CommandActor s_gameMaster =
        new("gm", FrontEnd.InGame, new HashSet<string> { PermissionKeys.CharacterRelocate }, "192.0.2.2");
    private static readonly CommandActor s_player = new("player", FrontEnd.InGame, new HashSet<string>(), "192.0.2.1");

    private readonly FakeWriteBehind _db = new();
    private readonly FakeRealmInbox _realm = new();
    private readonly CommandDispatcher _dispatcher;

    public CharacterCommandsTests()
    {
        var time = new FakeTimeProvider(new DateTimeOffset(2026, 10, 16, 12, 0, 0, TimeSpan.Zero));
        _dispatcher = new CommandDispatcher(CharacterCommands.Create(_realm), _db, time, NullLogger<CommandDispatcher>.Instance);
    }

    [Fact]
    public async Task TheCommandRelocatesThroughTheRealm()
    {
        _realm.OnPost = message => ((RelocateRequested)message).Reply.TrySetResult(true);

        CommandReply reply = await _dispatcher.DispatchAsync(s_console, "character relocate 42 5 6 7", CancellationToken.None);

        Assert.Equal(CommandOutcome.Done, reply.Outcome);
        Assert.Equal("relocated", reply.Text);
        RelocateRequested request = Assert.IsType<RelocateRequested>(Assert.Single(_realm.Posted));
        Assert.Equal(42UL, request.CharacterId);
        Assert.Equal(new Vector3(5, 6, 7), request.Position);
        Assert.Equal(0f, request.Facing);
        AuditRow audit = Assert.Single(_db.Queued.OfType<AuditWork>()).Row;
        Assert.Equal("character relocate", audit.Action);
        Assert.Equal("Done", audit.Outcome);
    }

    [Fact]
    public async Task AGameMasterRelocatesInGameWithAFacing()
    {
        _realm.OnPost = message => ((RelocateRequested)message).Reply.TrySetResult(true);

        CommandReply reply = await _dispatcher.DispatchAsync(s_gameMaster, "character relocate 42 5 6 7 1.5", CancellationToken.None);

        Assert.Equal(CommandOutcome.Done, reply.Outcome);
        Assert.Equal(1.5f, Assert.IsType<RelocateRequested>(Assert.Single(_realm.Posted)).Facing);
    }

    [Fact]
    public async Task ACharacterNotInTheWorldIsRefused()
    {
        _realm.OnPost = message => ((RelocateRequested)message).Reply.TrySetResult(false);

        CommandReply reply = await _dispatcher.DispatchAsync(s_console, "character relocate 42 5 6 7", CancellationToken.None);

        Assert.Equal(CommandOutcome.Refused, reply.Outcome);
        Assert.Equal("the character is not in the world", reply.Text);
    }

    [Fact]
    public async Task TheCommandIsRefusedWithoutItsPermission()
    {
        CommandReply reply = await _dispatcher.DispatchAsync(s_player, "character relocate 42 5 6 7", CancellationToken.None);

        Assert.Equal(CommandOutcome.Denied, reply.Outcome);
        Assert.Empty(_realm.Posted);
        Assert.Equal("Denied", Assert.Single(_db.Queued.OfType<AuditWork>()).Row.Outcome);
    }

    [Theory]
    [InlineData("character relocate -1 5 6 7")]
    [InlineData("character relocate 42 five 6 7")]
    [InlineData("character relocate 42 5 6 NaN")]
    [InlineData("character relocate 42 5 6 7 Infinity")]
    public async Task ABadNumberIsRefusedBeforeTheRealmHearsOfIt(string line)
    {
        Assert.Equal(CommandOutcome.Refused, (await _dispatcher.DispatchAsync(s_console, line, CancellationToken.None)).Outcome);
        Assert.Empty(_realm.Posted);
    }
}
