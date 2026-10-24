using System.Net;
using System.Numerics;
using Microsoft.Extensions.Logging;
using NexusTomorrow.Maps.Movement;
using NexusTomorrow.Maps.Visibility;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Entity.Command;
using NexusTomorrow.Network.World.Message.Model.Entity;
using NexusTomorrow.Network.World.Message.Model.Movement;
using NexusTomorrow.Owners;
using NexusTomorrow.Owners.World;
using NexusTomorrow.Persistence.Realm;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Maps.Tests;

public sealed class RelayTests : IDisposable
{
    private const int TicksPerFlush = 5;

    private readonly FakeWriteBehind _persistence = new();
    private readonly MapMetrics _metrics = new();
    private readonly FakeRealmInbox _realm = new();
    private readonly LinesLogger _log = new();
    private long _now;
    private int _ticks;

    public void Dispose() => _metrics.Dispose();

    [Fact]
    public void AMoveReachesTheViewerAtTheFlushWithTheSendersTime()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        FakeClientSession b = Enter(map, 1002, new Vector3(10, 0, 0));
        Flush(map);
        Grant(map, a);

        Batch(map, a, 5000, Time(5250), Position(new Vector3(1, 2, 3)), Facing(0.5f));
        Assert.Empty(b.Sent.OfType<UnitMovement>());
        Flush(map);

        UnitMovement moved = Assert.Single(b.Sent.OfType<UnitMovement>());
        Assert.Equal(OwnUnit(a), moved.UnitId);
        Assert.Equal(5000u, moved.Time);
        Assert.False(moved.ServerMoved);
        byte[] arms = [0, 2, 14];
        Assert.Equal(arms, moved.Commands.Select(command => command.Unknown0));
        Assert.Equal(new Vector3(1, 2, 3), Assert.IsType<SetPositionCommand>(moved.Commands[1].Unknown1).Position);
    }

    [Fact]
    public void TheFirstMoveAfterACreateResetsForThatViewerOnly()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        FakeClientSession b = Enter(map, 1002, new Vector3(10, 0, 0));
        Flush(map);
        Grant(map, a);
        Batch(map, a, 5000, Position(Vector3.One));
        Flush(map);

        FakeClientSession c = Enter(map, 1003, new Vector3(5, 0, 0));
        Batch(map, a, 5125, Position(Vector3.One));
        Flush(map);

        Assert.False(b.Sent.OfType<UnitMovement>().Last().Reset);
        Assert.True(Assert.Single(c.Sent.OfType<UnitMovement>()).Reset);
    }

    [Fact]
    public void TheFirstMoveAfterAGrantResetsForEveryViewer()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        FakeClientSession b = Enter(map, 1002, new Vector3(10, 0, 0));
        FakeClientSession c = Enter(map, 1003, new Vector3(5, 0, 0));
        Flush(map);
        Grant(map, a);

        Batch(map, a, 5000, Position(Vector3.One));
        Flush(map);

        Assert.True(Assert.Single(b.Sent.OfType<UnitMovement>()).Reset);
        Assert.True(Assert.Single(c.Sent.OfType<UnitMovement>()).Reset);
    }

    [Fact]
    public void TheSecondMoveCarriesNoReset()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        FakeClientSession b = Enter(map, 1002, new Vector3(10, 0, 0));
        Flush(map);
        Grant(map, a);
        Batch(map, a, 5000, Position(Vector3.One));
        Flush(map);

        Batch(map, a, 5125, Position(Vector3.One));
        Flush(map);

        UnitMovement second = b.Sent.OfType<UnitMovement>().Last();
        Assert.Equal(5125u, second.Time);
        Assert.False(second.Reset);
    }

    [Fact]
    public void TwoBatchesInOneIntervalGoOutAsTwoMessagesInOrder()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        FakeClientSession b = Enter(map, 1002, new Vector3(10, 0, 0));
        Flush(map);
        Grant(map, a);

        Batch(map, a, 5000, Position(Vector3.One));
        Batch(map, a, 5100, Position(Vector3.UnitX));
        Flush(map);

        uint[] times = [5000, 5100];
        Assert.Equal(times, b.Sent.OfType<UnitMovement>().Select(moved => moved.Time));
    }

    [Fact]
    public void AnOlderBatchIsDroppedAndCounted()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        FakeClientSession b = Enter(map, 1002, new Vector3(10, 0, 0));
        Flush(map);
        Grant(map, a);
        Batch(map, a, 5100, Position(Vector3.One));
        Flush(map);

        Batch(map, a, 5000, Position(Vector3.One));
        Flush(map);

        Assert.Single(b.Sent.OfType<UnitMovement>());
        Assert.Equal(1L, map.DroppedBatches(RelayDrop.Older));
    }

    [Fact]
    public void ALostBatchMakesTheNextRelayReset()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        FakeClientSession b = Enter(map, 1002, new Vector3(10, 0, 0));
        Flush(map);
        Grant(map, a);
        Batch(map, a, 4000, Position(Vector3.One));
        Flush(map);

        for (uint i = 0; i < 17; i++)
            Batch(map, a, 5000 + i, Position(Vector3.One));
        Flush(map);

        Assert.Equal(17, b.Sent.OfType<UnitMovement>().Count());
        Assert.Equal(1L, map.DroppedBatches(RelayDrop.RingFull));
        Assert.All(b.Sent.OfType<UnitMovement>().Skip(1), moved => Assert.False(moved.Reset));

        Batch(map, a, 6000, Position(Vector3.One));
        Flush(map);
        Assert.True(b.Sent.OfType<UnitMovement>().Last().Reset);
    }

    [Fact]
    public void ABatchOverThirtyOneCommandsIsDroppedAndTheNextResets()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        FakeClientSession b = Enter(map, 1002, new Vector3(10, 0, 0));
        Flush(map);
        Grant(map, a);
        Batch(map, a, 4000, Position(Vector3.One));
        Flush(map);

        Batch(map, a, 5000, [.. Enumerable.Range(0, 32).Select(_ => Position(Vector3.One))]);
        Flush(map);

        Assert.Single(b.Sent.OfType<UnitMovement>());
        Assert.Equal(1L, map.DroppedBatches(RelayDrop.TooLong));

        Batch(map, a, 5125, Position(Vector3.One));
        Flush(map);
        Assert.True(b.Sent.OfType<UnitMovement>().Last().Reset);
    }

    [Fact]
    public void AMoverNeverReceivesItsOwnMovement()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        Enter(map, 1002, new Vector3(10, 0, 0));
        Flush(map);
        Grant(map, a);

        Batch(map, a, 5000, Position(Vector3.One));
        Flush(map);

        Assert.Empty(a.Sent.OfType<UnitMovement>());
    }

    [Fact]
    public void AMoveFromAPlayerWithoutControlIsNotRelayed()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        FakeClientSession b = Enter(map, 1002, new Vector3(10, 0, 0));
        Flush(map);

        Batch(map, b, 5000, Position(Vector3.One));
        Flush(map);

        Assert.Empty(a.Sent.OfType<UnitMovement>());
    }

    [Fact]
    public void ARelayTheGateRefusesIsDroppedAndEverySessionStays()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        FakeClientSession b = Enter(map, 1002, new Vector3(10, 0, 0));
        Flush(map);
        Grant(map, a);

        // Arm 9's Unknown4 is unmarked, so the send gate refuses a relay that sets it.
        Batch(map, a, 5000, new Server0262Part5 { Unknown0 = 9, Unknown1 = new Server0262Union2Arm09 { Unknown4 = 1 } });
        Flush(map);

        Assert.Empty(b.Sent.OfType<UnitMovement>());
        Assert.Equal(1L, map.DroppedBatches(RelayDrop.Refused));
        Assert.Contains(_log.Lines, line => line.Contains("Unknown4", StringComparison.Ordinal));
        Assert.Null(a.DisconnectReason);
        Assert.Null(b.DisconnectReason);
    }

    [Fact]
    public void ARemoteCreateCarriesTheMoversLatestMotion()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        Enter(map, 1002, new Vector3(10, 0, 0));
        Flush(map);
        Grant(map, a);
        Batch(map, a, 5000, Velocity(new Vector3(1, 0, 0)), Moving(new Vector3(2, 0, 0)), State(3));
        Batch(map, a, 5100, Velocity(new Vector3(4, 0, 0)));
        Flush(map);

        FakeClientSession c = Enter(map, 1003, new Vector3(5, 0, 0));
        Flush(map);

        UnitCreate create = Assert.Single(c.Sent.OfType<UnitCreate>(), sent => sent.UnitId == OwnUnit(a));
        byte[] arms = [2, 14, 8, 11, 24];
        Assert.Equal(arms, create.Commands.Select(command => command.Unknown0));
        Assert.Equal(new Vector3(4, 0, 0), Assert.IsType<SetVelocityCommand>(create.Commands[2].Unknown1).Velocity);
        Assert.Equal(new Vector3(2, 0, 0), Assert.IsType<SetMoveCommand>(create.Commands[3].Unknown1).CommandedVelocity);
        Assert.Equal(3u, Assert.IsType<SetStateCommand>(create.Commands[4].Unknown1).State);
        // The create goes out at this flush: the mover's last relayed time, moved on by the time since that batch arrived.
        Assert.Equal((uint)(5100 + (_now - ArrivedAt(5100))), create.SenderTime);
    }

    [Fact]
    public void ATickThatRelaysAllocatesNothing()
    {
        MapOwner map = NewMap();
        var a = new CountingSession();
        var b = new CountingSession();
        Enter(map, a, 1001, Vector3.Zero);
        Enter(map, b, 1002, new Vector3(10, 0, 0));
        Flush(map);
        Grant(map, a, 1);
        Grant(map, b, 2);

        uint clock = 5000;
        for (int warmUp = 0; warmUp < 10; warmUp++)
            Interval(map, a, b, ref clock, measure: false);
        Assert.True(b.Received > 0);

        for (int flush = 0; flush < 50; flush++)
            Interval(map, a, b, ref clock, measure: true);
    }

    private static uint OwnUnit(FakeClientSession session) => session.Single<PlayerUnit>().UnitId;

    private static Server0262Part5 Time(uint time) => new() { Unknown0 = 0, Unknown1 = new SetTimeCommand { Time = time } };

    private static Server0262Part5 Position(Vector3 position) => new() { Unknown0 = 2, Unknown1 = new SetPositionCommand { Position = position } };

    private static Server0262Part5 Facing(float yaw) => new() { Unknown0 = 14, Unknown1 = new SetRotationCommand { Rotation = new Vector3(yaw, 0, 0) } };

    private static Server0262Part5 Velocity(Vector3 velocity) => new() { Unknown0 = 8, Unknown1 = new SetVelocityCommand { Velocity = velocity } };

    private static Server0262Part5 Moving(Vector3 velocity) => new() { Unknown0 = 11, Unknown1 = new SetMoveCommand { CommandedVelocity = velocity } };

    private static Server0262Part5 State(uint state) => new() { Unknown0 = 24, Unknown1 = new SetStateCommand { State = state } };

    private readonly Dictionary<uint, long> _batchArrivals = [];

    private long ArrivedAt(uint time) => _batchArrivals[time];

    private MapOwner NewMap() =>
        new(1, MapOwnerTests.Settings() with { AdmitRadius = 20, DropRadius = 30 }, _persistence, new Random(1), _metrics,
            new FakeMessageSerializer(), AllowAllVisibility.Instance, _log);

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

    private void Grant(MapOwner map, FakeClientSession session) => Grant(map, session, session.Single<MovementControl>().Ticket);

    private void Grant(MapOwner map, IWorldSession session, uint ticket)
    {
        map.Post(new ClientControlReceipt(session, ticket));
        Tick(map);
    }

    // Posts one movement batch, keeping the map's time at which it arrived (the next tick's).
    private void Batch(MapOwner map, IWorldSession session, uint time, params Server0262Part5[] commands)
    {
        map.PostHigh(new ClientMoved(session, new MovementSample(null, null), new RelayBatch(time, commands)));
        _batchArrivals[time] = _now + 25;
    }

    // One replication interval with both players moving; with measure, every tick of it must allocate nothing.
    private void Interval(MapOwner map, CountingSession a, CountingSession b, ref uint clock, bool measure)
    {
        clock += 125;
        Server0262Part5[] commands = [Time(clock), Position(new Vector3(1, 2, 3)), Facing(0.5f)];
        map.PostHigh(new ClientMoved(a, new MovementSample(new Vector3(1, 2, 3), 0.5f), new RelayBatch(clock, commands)));
        map.PostHigh(new ClientMoved(b, new MovementSample(new Vector3(9, 2, 3), 0.5f), new RelayBatch(clock, commands)));
        for (int i = 0; i < TicksPerFlush; i++)
        {
            Tick(map);
            if (measure)
                Assert.Equal(0L, map.LastTickAllocatedBytes);
        }
    }

    // A session that only counts what it is sent, so a tick's measured allocation is the map's own.
    private sealed class CountingSession : IWorldSession
    {
        private static long s_nextId = 1_000_000;

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

    private sealed class LinesLogger : ILogger<MapOwner>
    {
        private readonly List<string> _lines = [];

        public IReadOnlyList<string> Lines => _lines;

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
            _lines.Add(formatter(state, exception) + " " + exception?.Message);
    }
}
