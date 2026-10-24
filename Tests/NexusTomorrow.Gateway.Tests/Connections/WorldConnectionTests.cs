using System.Net;
using System.Numerics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Gateway.Connections;
using NexusTomorrow.Network;
using NexusTomorrow.Network.Message.Model;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Entity.Command;
using NexusTomorrow.Network.World.Message.Model.AccountInventory;
using NexusTomorrow.Network.World.Message.Model.Chat;
using NexusTomorrow.Network.World.Message.Model.Movement;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Network.World.Message.Model.WorldEntry;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Owners.Text;
using NexusTomorrow.Owners.World;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Account;
using NexusTomorrow.Persistence.Realm;
using NexusTomorrow.Testing;
using NexusTomorrow.Transport;

namespace NexusTomorrow.Gateway.Tests.Connections;

public sealed class WorldConnectionTests : IAsyncLifetime
{
    private static readonly AccountCredentials s_alice = new(7, "alice@example.com", [1], [2], null, false);

    private readonly FakeTimeProvider _time = new();
    private readonly FakeWriteBehind _db = new();
    private readonly FakeMapDirectory _maps = new();
    private readonly CapturingLogger _log = new();
    private readonly AccountOwner _accounts;
    private readonly RealmOwner _realm;
    private readonly PipePair _pipes = new();
    private readonly CancellationTokenSource _stop = new();
    private WorldConnection _connection = null!;
    private Task _run = Task.CompletedTask;

    public WorldConnectionTests()
    {
        _accounts = new AccountOwner(_time, Options.Create(new AccountOptions()), NullLogger<AccountOwner>.Instance, new FakeWriteBehind(), FixedEntitlementCaps.Defaults());
        _realm = new RealmOwner(_db, new FixedCreationCatalog([]), new WordFilter([], NullLogger<WordFilter>.Instance), FixedEntitlementCaps.Defaults(), _maps, _time, Options.Create(new RealmOptions { Id = 3 }), NullLogger<RealmOwner>.Instance);
    }

    private GameTestClient Client { get; set; } = null!;

    public ValueTask InitializeAsync()
    {
        _accounts.Start();
        _realm.Post(new IdBlockTaken(new IdBlock(1001, 1000)));
        _realm.Start();
        IOptions<GatewayOptions> gateway = Options.Create(new GatewayOptions { UnknownAccountSecret = Convert.ToBase64String(new byte[32]) });
        _connection = new WorldConnection(MessageCatalog.Default, new InboundDecoder(MessageCatalog.Default, OpcodeTable.All, new HashSet<ushort>()),
            _accounts, _realm, gateway, Options.Create(new RealmOptions { Id = 3 }), _time, new GatewayMetrics(), _log);
        _run = _connection.RunAsync(_pipes.Server, new IPEndPoint(IPAddress.Loopback, 50002), _stop.Token);
        Client = new GameTestClient(_pipes.Client);
        return ValueTask.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await _stop.CancelAsync();
        await _run.WaitAsync(TimeSpan.FromSeconds(10));
        await _realm.StopAsync().WaitAsync(TimeSpan.FromSeconds(10));
        await _accounts.StopAsync().WaitAsync(TimeSpan.FromSeconds(10));
    }

    [Fact]
    public async Task TheGreetingTravelsInTheWorldEnvelopeUnderTheBuildKey()
    {
        Received greeting = await Client.ReceiveAsync();
        Assert.Equal(0x03DC, greeting.FrameOpcode);
        Assert.Equal(3u, Assert.IsType<ServerHello>(greeting.Message).RealmId);
    }

    [Fact]
    public async Task ARequestInTheSameWriteAsTheGreetingIsReadUnderTheNewKey()
    {
        await Client.ExpectAsync<ServerHello>();
        LoginGranted grant = await GrantAsync();

        byte[] greet = Client.Envelope(0x058F, Hello(grant));
        Client.Cipher = new PacketCipher(PacketCipher.SessionKey(grant.SessionKey));
        byte[] list = Client.Envelope(0x07E0, new CharacterListRequest());
        await Client.SendRawAsync([.. greet, .. list]);

        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharactersWork>().Any());
        LoadCharactersWork load = _db.Take<LoadCharactersWork>();
        load.Complete(WorkResult.Committed);
        await Client.ExpectAsync<AccountEntitlements>();
        await Client.ExpectAsync<CreationLevelLimit>();
        Assert.Empty((await Client.ExpectAsync<CharacterList>()).Characters);
    }

    [Fact]
    public async Task AnUnknownSessionKeyEndsTheConnection()
    {
        await Client.ExpectAsync<ServerHello>();
        await Client.SendAsync(Hello(new LoginGranted(7, new byte[16], 0)));
        Assert.True(await Client.IsClosedAsync(TimeSpan.FromSeconds(10)));
    }

    [Fact]
    public async Task ARefusedSessionKeyIsLoggedByReasonAndNeverByKey()
    {
        await Client.ExpectAsync<ServerHello>();
        LoginGranted grant = await GrantAsync();
        var wrong = new LoginGranted(grant.AccountId, grant.SessionKey, grant.Echo + 1);
        await Client.SendAsync(Hello(wrong));

        Assert.True(await Client.IsClosedAsync(TimeSpan.FromSeconds(10)));
        await Eventually.TrueAsync(() => _log.Lines.Any(line => line.Contains("wrong echo or account name", StringComparison.Ordinal)));
        Assert.DoesNotContain(_log.Lines, line => line.Contains(Convert.ToHexString(grant.SessionKey), StringComparison.OrdinalIgnoreCase)
            || line.Contains(Convert.ToBase64String(grant.SessionKey), StringComparison.Ordinal));
    }

    [Fact]
    public async Task AGreetingWhileTheAccountOwnerIsStoppedEndsTheConnection()
    {
        await Client.ExpectAsync<ServerHello>();
        await _accounts.StopAsync().WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken);
        await Client.SendAsync(Hello(new LoginGranted(7, new byte[16], 0)));
        Assert.True(await Client.IsClosedAsync(TimeSpan.FromSeconds(10)));
        await Eventually.TrueAsync(() => _log.Lines.Any(line => line.Contains("the server is stopping", StringComparison.Ordinal)));
    }

    [Fact]
    public async Task AMessageBeforeTheGreetingEndsTheConnection()
    {
        await Client.ExpectAsync<ServerHello>();
        await Client.SendAsync(new CharacterListRequest());
        Assert.True(await Client.IsClosedAsync(TimeSpan.FromSeconds(10)));
    }

    [Fact]
    public async Task CharacterCreateArrivesThroughTheNestedWrapper()
    {
        await GreetAsync();
        await Client.SendAsync(new CharacterListRequest());
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharactersWork>().Any());
        LoadCharactersWork load = _db.Take<LoadCharactersWork>();
        load.Complete(WorkResult.Committed);
        await Client.ExpectAsync<CharacterList>();

        await Client.SendNestedAsync(new CharacterCreate { CreationId = 1, Name = "Rebaser Head", Path = 1 });
        // The fixture catalog is empty, so the realm owner answers the general failure: the message reached it.
        Assert.Equal(CreateResults.General, (await Client.ExpectAsync<CharacterCreateResult>()).Result);
    }

    [Fact]
    public async Task TheDebugLogNamesTheInboundCharacterSelectAndTheOutboundCharacterSelectFailed()
    {
        await GreetAsync();
        await Client.SendAsync(new CharacterSelect { CharacterId = 424242 });
        await Client.ExpectAsync<CharacterSelectFailed>();

        Assert.Contains(_log.Lines, line => line.Contains("received 0x07DD: CharacterSelect", StringComparison.Ordinal));
        Assert.Contains(_log.Lines, line => line.Contains("sent 0x0162 CharacterSelectFailed", StringComparison.Ordinal));
        Assert.DoesNotContain(_log.Lines, line => line.Contains("424242", StringComparison.Ordinal));
    }

    [Fact]
    public async Task ClosingTheConnectionTakesTheAccountOffline()
    {
        await GreetAsync();
        await Eventually.TrueAsync(() => _accounts.OnlineCount == 1);
        await _pipes.Client.Output.CompleteAsync();
        await Eventually.TrueAsync(() => _accounts.OnlineCount == 0);
    }

    [Fact]
    public async Task AWorldMessageReachesTheMapTheSessionEnteredAndItsEndLeavesIt()
    {
        await GreetAsync();
        var map = new FakeMapInbox(1);
        Assert.True(_connection.TryEnterMap(map));

        await Client.SendTaggedAsync(0x03, ClientMessage.With(new WorldReady(), nameof(WorldReady.ZoneId), (ushort)6));
        await Eventually.TrueAsync(() => map.Posted.Count == 1);
        ClientWorldReady ready = Assert.IsType<ClientWorldReady>(map.Posted[0]);
        Assert.Equal(6, ready.ZoneId);
        Assert.Same(_connection, ready.Session);

        await _pipes.Client.Output.CompleteAsync();
        await Eventually.TrueAsync(() => map.Posted.Count == 2);
        Assert.Same(_connection, Assert.IsType<ClientLeft>(map.Posted[1]).Session);
    }

    [Fact]
    public async Task EveryMapRowReachesTheMapAsItsOwnMessage()
    {
        await GreetAsync();
        var map = new FakeMapInbox(1);
        Assert.True(_connection.TryEnterMap(map));

        await Client.SendTaggedAsync(0x03, ClientMessage.With(new MovementControlReceipt(), nameof(MovementControlReceipt.Ticket), 77u));
        await Client.SendTaggedAsync(0x03, ClientMessage.With(new ZoneChange(), nameof(ZoneChange.ZoneId), (ushort)9));
        await Eventually.TrueAsync(() => map.Posted.Count == 2);

        Assert.Equal(77u, Assert.IsType<ClientControlReceipt>(map.Posted[0]).Ticket);
        Assert.Equal(9, Assert.IsType<ClientZoneChanged>(map.Posted[1]).ZoneId);
    }

    [Fact]
    public async Task TheLogoutRequestsAndTheConfirmReachTheMap()
    {
        await GreetAsync();
        var map = new FakeMapInbox(1);
        Assert.True(_connection.TryEnterMap(map));

        await Client.SendTaggedAsync(0x03, ClientMessage.With(new LogoutRequest(), nameof(LogoutRequest.Begin), true));
        await Client.SendTaggedAsync(0x03, ClientMessage.With(new LogoutRequest(), nameof(LogoutRequest.Begin), false));
        await Client.SendTaggedAsync(0x03, new LogoutConfirm());
        await Eventually.TrueAsync(() => map.Posted.Count == 3);

        Assert.True(Assert.IsType<ClientLogoutRequested>(map.Posted[0]).Begin);
        Assert.False(Assert.IsType<ClientLogoutRequested>(map.Posted[1]).Begin);
        Assert.IsType<ClientLogoutConfirmed>(map.Posted[2]);
        Assert.All(map.Posted, posted => Assert.Same(_connection, Assert.IsAssignableFrom<SessionMessage>(posted).Session));
    }

    [Fact]
    public async Task AClientMovementReachesTheMapAtHighPriority()
    {
        await GreetAsync();
        var map = new FakeMapInbox(1);
        Assert.True(_connection.TryEnterMap(map));
        var movement = new ClientMovement { ClientTime = 1000 };
        movement.Commands.Add(new Server0262Part5 { Unknown0 = 2, Unknown1 = new SetPositionCommand { Position = new Vector3(1, 2, 3) } });

        await Client.SendTaggedAsync(0x03, movement);
        await Eventually.TrueAsync(() => map.Posted.Count == 1);

        ClientMoved moved = Assert.IsType<ClientMoved>(map.Posted[0]);
        Assert.Equal(new Vector3(1, 2, 3), moved.Sample.Position);
        Assert.Same(_connection, moved.Session);
        Assert.Equal(1, map.HighPosts);
    }

    [Fact]
    public async Task AClientMovementCarriesItsRelayBatchToTheMap()
    {
        await GreetAsync();
        var map = new FakeMapInbox(1);
        Assert.True(_connection.TryEnterMap(map));
        var movement = new ClientMovement { ClientTime = 5000 };
        movement.Commands.Add(new Server0262Part5 { Unknown0 = 0, Unknown1 = new SetTimeCommand { Time = 5250 } });
        movement.Commands.Add(new Server0262Part5 { Unknown0 = 2, Unknown1 = new SetPositionCommand { Position = new Vector3(1, 2, 3) } });

        await Client.SendTaggedAsync(0x03, movement);
        await Eventually.TrueAsync(() => map.Posted.Count == 1);

        RelayBatch batch = Assert.IsType<RelayBatch>(Assert.IsType<ClientMoved>(map.Posted[0]).Batch);
        Assert.Equal(5000u, batch.Time);
        byte[] arms = [0, 2];
        Assert.Equal(arms, batch.Commands.Select(command => command.Unknown0));
    }

    [Fact]
    public async Task AnArmOutsideTheConfirmedListDropsTheMessageAndTheSessionGoesOn()
    {
        await GreetAsync();
        var map = new FakeMapInbox(1);
        Assert.True(_connection.TryEnterMap(map));
        var refused = new ClientMovement { ClientTime = 1 };
        refused.Commands.Add(new Server0262Part5 { Unknown0 = 0, Unknown1 = new SetTimeCommand { Time = 1 } });
        refused.Commands.Add(new Server0262Part5 { Unknown0 = 9, Unknown1 = new Server0262Union2Arm09() });
        refused.Commands.Add(new Server0262Part5 { Unknown0 = 2, Unknown1 = new SetPositionCommand { Position = new Vector3(1, 2, 3) } });
        var accepted = new ClientMovement { ClientTime = 2 };
        accepted.Commands.Add(new Server0262Part5 { Unknown0 = 2, Unknown1 = new SetPositionCommand { Position = new Vector3(4, 5, 6) } });

        await Client.SendTaggedAsync(0x03, refused);
        await Client.SendTaggedAsync(0x03, accepted);
        await Eventually.TrueAsync(() => map.Posted.Count == 1);

        Assert.Contains(_log.Lines, line => line.Contains("an unconfirmed command arm", StringComparison.Ordinal));
        Assert.Equal(new Vector3(4, 5, 6), Assert.IsType<ClientMoved>(Assert.Single(map.Posted)).Sample.Position);
    }

    [Fact]
    public async Task ForcedMoveArmsAreLoggedOnceAndTheRestIsRouted()
    {
        await GreetAsync();
        var map = new FakeMapInbox(1);
        Assert.True(_connection.TryEnterMap(map));
        var movement = new ClientMovement { ClientTime = 3 };
        movement.Commands.Add(new Server0262Part5 { Unknown0 = 3, Unknown1 = new Server0262Union2Arm03() });
        movement.Commands.Add(new Server0262Part5 { Unknown0 = 2, Unknown1 = new SetPositionCommand { Position = new Vector3(1, 2, 3) } });
        movement.Commands.Add(new Server0262Part5 { Unknown0 = 25, Unknown1 = new Server0262Union2Arm19() });

        await Client.SendTaggedAsync(0x03, movement);
        await Eventually.TrueAsync(() => map.Posted.Count == 1);

        string forced = Assert.Single(_log.Lines, line => line.Contains("forced-move", StringComparison.Ordinal));
        Assert.Contains("Arm3", forced, StringComparison.Ordinal);
        Assert.Contains("Arm25", forced, StringComparison.Ordinal);
        byte[] arms = [2];
        Assert.Equal(arms, Assert.IsType<ClientMoved>(map.Posted[0]).Batch!.Commands.Select(command => command.Unknown0));
    }

    [Fact]
    public async Task AnOversizedClientMovementEndsTheSessionBeforeItIsRead()
    {
        await GreetAsync();
        var map = new FakeMapInbox(1);
        Assert.True(_connection.TryEnterMap(map));

        await Client.SendTaggedUncheckedAsync(0x03, 0x0637, new RawBody(new byte[2049]));

        await _run.WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken);
        Assert.Contains(_log.Lines, line => line.Contains("a body over its row's size limit", StringComparison.Ordinal));
        // Ending the session tells the map it left; no movement reached it.
        Assert.DoesNotContain(map.Posted, posted => posted is ClientMoved);
        Assert.IsType<ClientLeft>(Assert.Single(map.Posted));
    }

    [Fact]
    public async Task AWorldMessageWithNoMapIsDroppedAndTheSessionGoesOn()
    {
        await GreetAsync();
        await Client.SendTaggedAsync(0x03, ClientMessage.With(new ZoneChange(), nameof(ZoneChange.ZoneId), (ushort)9));
        await Client.SendAsync(new CharacterListRequest());
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharactersWork>().Any());
        _db.Take<LoadCharactersWork>().Complete(WorkResult.Committed);

        Assert.Empty((await Client.ExpectAsync<CharacterList>()).Characters);
        Assert.Contains(_log.Lines, line => line.Contains("dropped message 0x063A: a map message with no map", StringComparison.Ordinal));
    }

    [Fact]
    public async Task AMovementThatWouldBeMalformedIsDroppedUnreadWhileTheSessionIsInNoMap()
    {
        await GreetAsync();
        await Client.SendTaggedUncheckedAsync(0x03, 0x0637, new RawBody(ClientFrames.OverclaimingMovement()));
        await Client.SendAsync(new CharacterListRequest());

        // The list request reaches the realm only while the session is open.
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharactersWork>().Any() || _run.IsCompleted);
        Assert.DoesNotContain(_log.Lines, line => line.Contains("ended:", StringComparison.Ordinal));
        _db.Take<LoadCharactersWork>().Complete(WorkResult.Committed);

        Assert.Empty((await Client.ExpectAsync<CharacterList>()).Characters);
        Assert.Contains(_log.Lines, line => line.Contains("dropped message 0x0637: a map message with no map", StringComparison.Ordinal));
    }

    [Fact]
    public async Task AWorldMessageAfterTheMapLeavesTheSessionIsDroppedAndTheSessionGoesOn()
    {
        await GreetAsync();
        var map = new FakeMapInbox(1);
        Assert.True(_connection.TryEnterMap(map));
        await Client.SendTaggedAsync(0x03, ClientMessage.With(new WorldReady(), nameof(WorldReady.ZoneId), (ushort)6));
        await Eventually.TrueAsync(() => map.Posted.Count == 1);

        _connection.LeaveMap(map);
        await Client.SendTaggedAsync(0x03, ClientMessage.With(new ZoneChange(), nameof(ZoneChange.ZoneId), (ushort)9));
        await Client.SendAsync(new CharacterListRequest());
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharactersWork>().Any());
        _db.Take<LoadCharactersWork>().Complete(WorkResult.Committed);

        // The list answers after the zone change was read, so the zone change was handled by then.
        Assert.Empty((await Client.ExpectAsync<CharacterList>()).Characters);
        Assert.Contains(_log.Lines, line => line.Contains("dropped message 0x063A: a map message with no map", StringComparison.Ordinal));
        await _pipes.Client.Output.CompleteAsync();
        await _run.WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken);
        Assert.Single(map.Posted);
    }

    [Fact]
    public async Task AMapThatHasStoppedRefusesAPostAndTheSessionGoesOnWithNoDropCounted()
    {
        await GreetAsync();
        var map = new FakeMapInbox(1) { Accepting = false };
        Assert.True(_connection.TryEnterMap(map));
        await Client.SendTaggedAsync(0x03, ClientMessage.With(new WorldReady(), nameof(WorldReady.ZoneId), (ushort)6));
        await Client.SendAsync(new CharacterListRequest());
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharactersWork>().Any());
        _db.Take<LoadCharactersWork>().Complete(WorkResult.Committed);

        Assert.Empty((await Client.ExpectAsync<CharacterList>()).Characters);
        Assert.Empty(map.Posted);
        Assert.DoesNotContain(_log.Lines, line => line.Contains("dropped message 0x00F2", StringComparison.Ordinal));
    }

    [Fact]
    public async Task ACharacterSelectWhileInTheWorldIsDroppedAndChangesNothing()
    {
        await GreetAsync();
        Assert.True(_connection.TryEnterMap(new FakeMapInbox(1)));
        await Client.SendAsync(new CharacterSelect { CharacterId = 424242 });
        await Client.SendAsync(new CharacterListRequest());
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharactersWork>().Any());
        _db.Take<LoadCharactersWork>().Complete(WorkResult.Committed);

        // A select the realm owner saw would answer CharacterSelectFailed first; the list is the first answer.
        Assert.Empty((await Client.ExpectAsync<CharacterList>()).Characters);
        Assert.Contains(_log.Lines, line => line.Contains("dropped message 0x07DD: a select while in the world", StringComparison.Ordinal));
    }

    [Fact]
    public async Task ASayRoutesToTheRealmAsChatSaid()
    {
        await GreetAsync();
        FakeMapInbox map = await EnterThroughTheRealmAsync();

        await Client.SendTaggedAsync(0x0B, Say(4, "hello", 3));

        // The realm owner is the connection's own; what it was handed shows in what it passes to the speaker's map.
        await Eventually.TrueAsync(() => map.Posted.Count == 2);
        LocalChat chat = Assert.IsType<LocalChat>(map.Posted[1]);
        Assert.Same(_connection, chat.Session);
        Assert.Equal(5UL, chat.CharacterId);
        Assert.Equal((ushort)3, chat.ChatId);
        Assert.Equal("hello", chat.Text);
    }

    [Fact]
    public async Task ASayUnderTheWorldTagEndsTheSession()
    {
        await GreetAsync();
        var map = new FakeMapInbox(1);
        Assert.True(_connection.TryEnterMap(map));

        await Client.SendTaggedAsync(0x03, Say(4, "hello", 3));

        await _run.WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken);
        Assert.Contains(_log.Lines, line => line.Contains("a tag other than its row's", StringComparison.Ordinal));
    }

    [Fact]
    public async Task ASayOutsideTheWorldIsDropped()
    {
        await GreetAsync();
        await Client.SendTaggedAsync(0x0B, Say(4, "hello", 3));
        await Client.SendAsync(new CharacterListRequest());
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharactersWork>().Any());
        _db.Take<LoadCharactersWork>().Complete(WorkResult.Committed);

        Assert.Empty((await Client.ExpectAsync<CharacterList>()).Characters);
        Assert.Contains(_log.Lines, line => line.Contains("dropped message 0x01C3: chat outside the world", StringComparison.Ordinal));
    }

    [Fact]
    public async Task AnEndedConnectionEntersNoMap()
    {
        await Client.ExpectAsync<ServerHello>();
        await _pipes.Client.Output.CompleteAsync();
        await _run.WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken);

        Assert.False(_connection.TryEnterMap(new FakeMapInbox(1)));
    }

    private async Task GreetAsync()
    {
        await Client.ExpectAsync<ServerHello>();
        LoginGranted grant = await GrantAsync();
        await Client.SendAsync(Hello(grant));
        Client.Cipher = new PacketCipher(PacketCipher.SessionKey(grant.SessionKey));

        // The account owner's grant sends these before the session reads anything else.
        await Client.ExpectAsync<AccountEntitlements>();
        await Client.ExpectAsync<CreationLevelLimit>();
    }

    // Enters character 5 as the client does, through the realm's list and select, and routes the session to the map it arrived at.
    private async Task<FakeMapInbox> EnterThroughTheRealmAsync()
    {
        FakeMapInbox map = _maps.Add(1);
        await Client.SendAsync(new CharacterListRequest());
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharactersWork>().Any());
        LoadCharactersWork list = _db.Take<LoadCharactersWork>();
        list.Characters =
        [
            new CharacterSummary(5, s_alice.Id, "Rebaser Head", Race: 1, Sex: 0, Class: 1, FactionId: 166, Path: 0, Level: 1, WorldId: 1,
                ZoneId: 0, Position: default, Disabled: false, RequiresRename: false, Bones: []),
        ];
        list.Complete(WorkResult.Committed);
        await Client.ExpectAsync<CharacterList>();

        await Client.SendAsync(new CharacterSelect { CharacterId = 5 });
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharacterWork>().Any());
        LoadCharacterWork load = _db.Take<LoadCharacterWork>();
        load.Character = new CharacterRecord(
            5, s_alice.Id, "Rebaser Head", Race: 1, Sex: 0, Class: 1, FactionId: 166, Path: 0, Level: 1, TotalXp: 0,
            new CharacterLocation(1, 0, Vector3.Zero, 0));
        load.Complete(WorkResult.Committed);
        await Eventually.TrueAsync(() => map.Posted.Count == 1);
        Assert.IsType<CharacterArrived>(map.Posted[0]);
        Assert.True(_connection.TryEnterMap(map));
        return map;
    }

    private static ChatSend Say(ushort channelType, string text, ushort chatId)
    {
        ChatSend say = ClientMessage.With(ClientMessage.With(new ChatSend(), nameof(ChatSend.Text), text), nameof(ChatSend.ChatId), chatId);
        say.Channel.Type = channelType;
        return say;
    }

    private async Task<LoginGranted> GrantAsync()
    {
        var issued = new TaskCompletionSource<Guid>(TaskCreationOptions.RunContinuationsAsynchronously);
        _accounts.Post(new IssueGameToken(s_alice, issued));
        var granted = new TaskCompletionSource<LoginDecision>(TaskCreationOptions.RunContinuationsAsynchronously);
        _accounts.Post(new RedeemGameToken(await issued.Task.WaitAsync(TimeSpan.FromSeconds(10)), "alice@example.com", granted));
        return Assert.IsType<LoginGranted>(await granted.Task.WaitAsync(TimeSpan.FromSeconds(10)));
    }

    private static WorldHello Hello(LoginGranted grant)
    {
        var hello = new WorldHello { AccountId = grant.Echo, AccountName = "alice@example.com" };
        var key = NetworkGuid.FromBytes(grant.SessionKey);
        hello.SessionKey.Data1 = key.Data1;
        hello.SessionKey.Data2 = key.Data2;
        hello.SessionKey.Data3 = key.Data3;
        hello.SessionKey.Data4 = key.Data4;
        return hello;
    }

    private sealed class CapturingLogger : ILogger<WorldConnection>
    {
        private readonly List<string> _lines = [];
        private readonly Lock _gate = new();

        public IReadOnlyList<string> Lines
        {
            get
            {
                lock (_gate)
                    return [.. _lines];
            }
        }

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            lock (_gate)
                _lines.Add(formatter(state, exception));
        }
    }
}
