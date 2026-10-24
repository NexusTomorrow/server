using System.Net;
using System.Numerics;
using Microsoft.Extensions.Logging.Abstractions;
using NexusTomorrow.Maps.Visibility;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Chat;
using NexusTomorrow.Network.World.Message.Model.Entity;
using NexusTomorrow.Owners;
using NexusTomorrow.Owners.World;
using NexusTomorrow.Persistence.Realm;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Maps.Tests;

public sealed class LocalChatTests : IDisposable
{
    private const int TicksPerFlush = 5;

    private readonly FakeWriteBehind _persistence = new();
    private readonly MapMetrics _metrics = new();
    private readonly FakeRealmInbox _realm = new();
    private readonly FakeMessageSerializer _serializer = new();
    private long _now;
    private int _ticks;

    public void Dispose() => _metrics.Dispose();

    [Fact]
    public void TheSpeakerIsAcceptedWithItsOwnChatId()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        Flush(map);

        map.Post(new LocalChat(a, 1001, 3, "hello"));
        Tick(map);

        Assert.Equal((ushort)3, a.Single<ChatSendAccepted>().ChatId);
        Assert.Empty(a.Sent.OfType<ChatMessageReceived>());
    }

    [Fact]
    public void TheAcceptCarriesTheSpeakersNameRealmAndUnit()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        Flush(map);

        map.Post(new LocalChat(a, 1001, 3, "hello"));
        Tick(map);

        ChatSendAccepted accepted = a.Single<ChatSendAccepted>();
        Assert.Equal("Player 1001", accepted.SenderName);
        Assert.Equal("Fixture", accepted.RealmName);
        Assert.Equal(OwnUnit(a), accepted.UnitId);
    }

    [Fact]
    public void AListenerInRangeHearsTheLineOnSay()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        FakeClientSession b = Enter(map, 1002, new Vector3(100, 0, 0));
        Flush(map);

        map.Post(new LocalChat(a, 1001, 3, "hello"));
        Tick(map);

        ChatMessageReceived heard = b.Single<ChatMessageReceived>();
        Assert.Equal((ushort)4, heard.Channel.Type);
        Assert.Equal(0UL, heard.Channel.Id);
        Assert.Equal((ushort)11, heard.SenderId.RealmId);
        Assert.Equal(1001UL, heard.SenderId.Id);
        Assert.Equal("Player 1001", heard.SenderName);
        Assert.Equal("hello", heard.Text);
        Assert.Equal((ulong)OwnUnit(a), heard.UnitId);
    }

    [Fact]
    public void AListenerOutOfRangeHearsNothing()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        FakeClientSession far = Enter(map, 1003, new Vector3(200, 0, 0));
        FakeClientSession above = Enter(map, 1004, new Vector3(100, 120, 0));
        Flush(map);

        map.Post(new LocalChat(a, 1001, 3, "hello"));
        Tick(map);

        Assert.Empty(far.Sent.OfType<ChatMessageReceived>());
        Assert.Empty(above.Sent.OfType<ChatMessageReceived>());
    }

    [Fact]
    public void ALineIsSerializedOnceForEveryListener()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        FakeClientSession b = Enter(map, 1002, new Vector3(100, 0, 0));
        FakeClientSession d = Enter(map, 1005, new Vector3(0, 0, 50));
        Flush(map);

        map.Post(new LocalChat(a, 1001, 3, "hello"));
        Tick(map);

        Assert.Equal(1, _serializer.SerializedCount(typeof(ChatMessageReceived)));
        b.Single<ChatMessageReceived>();
        d.Single<ChatMessageReceived>();
    }

    [Fact]
    public void ALocalChatForAnotherCharacterIsIgnored()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        FakeClientSession b = Enter(map, 1002, new Vector3(100, 0, 0));
        Flush(map);

        map.Post(new LocalChat(a, 1002, 3, "hello"));
        Tick(map);

        Assert.Empty(a.Sent.OfType<ChatSendAccepted>());
        Assert.Empty(b.Sent.OfType<ChatMessageReceived>());
    }

    [Fact]
    public void ASpeakerNotYetInTheWorldIsIgnored()
    {
        MapOwner map = NewMap();
        var a = new FakeClientSession();
        map.Post(new CharacterArrived(a, 7, MapOwnerTests.Record(1001), _realm));
        FakeClientSession b = Enter(map, 1002, new Vector3(100, 0, 0));
        Flush(map);

        map.Post(new LocalChat(a, 1001, 3, "hello"));
        Tick(map);

        Assert.Empty(a.Sent.OfType<ChatSendAccepted>());
        Assert.Empty(b.Sent.OfType<ChatMessageReceived>());
    }

    [Fact]
    public void ASayLineAllocatesNothingOnTheTick()
    {
        MapOwner map = NewMap();
        var a = new CountingSession();
        var b = new CountingSession();
        Enter(map, a, 1001, Vector3.Zero);
        Enter(map, b, 1002, new Vector3(100, 0, 0));
        Flush(map);

        for (int warmUp = 0; warmUp < 10; warmUp++)
        {
            map.Post(new LocalChat(a, 1001, 3, "hello"));
            Tick(map);
        }

        Assert.True(b.Received > 0);
        for (int line = 0; line < 50; line++)
        {
            map.Post(new LocalChat(a, 1001, 3, "hello"));
            Tick(map);
            Assert.Equal(0L, map.LastTickAllocatedBytes);
        }
    }

    private static uint OwnUnit(FakeClientSession session) => session.Single<PlayerUnit>().UnitId;

    private MapOwner NewMap() =>
        new(1, MapOwnerTests.Settings() with { SayRange = 155, RealmName = "Fixture" }, _persistence, new Random(1), _metrics, _serializer,
            AllowAllVisibility.Instance, NullLogger<MapOwner>.Instance);

    private void Tick(MapOwner map)
    {
        _now += 25;
        _ticks++;
        map.Tick(_now);
    }

    private void Flush(MapOwner map)
    {
        do
            Tick(map);
        while (_ticks % TicksPerFlush != 0);
    }

    private FakeClientSession Enter(MapOwner map, ulong id, Vector3 at)
    {
        var session = new FakeClientSession();
        Enter(map, session, id, at);
        return session;
    }

    // Posts only, so players entered together are handled in one tick and no flush runs between them.
    private void Enter(MapOwner map, IWorldSession session, ulong id, Vector3 at)
    {
        CharacterRecord record = MapOwnerTests.Record(id) with { Name = $"Player {id}", Location = new CharacterLocation(1, 3, at, 0) };
        map.Post(new CharacterArrived(session, 7, record, _realm));
        map.Post(new ClientWorldReady(session, 3));
    }

    // A session that only counts what it is sent, so a tick's measured allocation is the map's own.
    private sealed class CountingSession : IWorldSession
    {
        private static long s_nextId = 2_000_000;

        public long Id { get; } = Interlocked.Increment(ref s_nextId);

        public IPAddress RemoteAddress => IPAddress.Loopback;

        public int Received { get; private set; }

        public void Send<T>(T message)
            where T : class, IWireMessage
        {
        }

        public void Send(SerializedMessage message)
        {
            message.AddReference();
            message.Release();
            Received++;
        }

        public void Disconnect(string reason)
        {
        }

        public bool TryEnterMap(IMapInbox map) => true;

        public void LeaveMap(IMapInbox map)
        {
        }
    }
}
