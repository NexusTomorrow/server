using System.Numerics;
using Microsoft.Extensions.Logging.Abstractions;
using NexusTomorrow.Maps.Visibility;
using NexusTomorrow.Network.World.Entity.Command;
using NexusTomorrow.Network.World.Entity.Model;
using NexusTomorrow.Network.World.Message.Model.Entity;
using NexusTomorrow.Network.World.Message.Model.Movement;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Owners.World;
using NexusTomorrow.Persistence.Realm;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Maps.Tests;

public sealed class VisibilityTests : IDisposable
{
    // The flush runs on every fifth tick of the map (ReplicationEveryTicks 5 in MapOwnerTests.Settings).
    private const int TicksPerFlush = 5;

    private readonly FakeWriteBehind _persistence = new();
    private readonly MapMetrics _metrics = new();
    private readonly FakeRealmInbox _realm = new();
    private readonly FakeMessageSerializer _serializer = new();
    private readonly HiddenPairs _hidden = new();
    private long _now;
    private int _ticks;

    public void Dispose() => _metrics.Dispose();

    [Fact]
    public void TwoPlayersInRangeSeeEachOtherAtTheFlush()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, new Vector3(0, 0, 0));
        FakeClientSession b = Enter(map, 1002, new Vector3(10, 0, 0));

        Flush(map);

        UnitCreate seenByA = Assert.Single(RemoteCreates(a));
        Assert.Equal(OwnUnit(b), seenByA.UnitId);
        Assert.Equal((byte)20, seenByA.UnitType);
        Assert.Equal("Player 1002", Assert.IsType<Server0262Union0Arm14>(seenByA.TypeData).Name);
        Assert.Equal(new Vector3(10, 0, 0), Assert.IsType<SetPositionCommand>(seenByA.Commands[0].Unknown1).Position);
        Assert.Equal((byte)14, seenByA.Commands[1].Unknown0);
        UnitPathTypeChanged pathForA = Assert.Single(RemotePaths(a));
        Assert.Equal(OwnUnit(b), pathForA.UnitId);
        Assert.Equal((byte)2, pathForA.PathType);
        Assert.True(IndexOf(a, seenByA) < IndexOf(a, pathForA));

        Assert.Equal(OwnUnit(a), Assert.Single(RemoteCreates(b)).UnitId);
        Assert.Equal(OwnUnit(a), Assert.Single(RemotePaths(b)).UnitId);
    }

    [Fact]
    public void NothingIsSentBetweenFlushes()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, new Vector3(0, 0, 0));
        FakeClientSession b = Enter(map, 1002, new Vector3(10, 0, 0));

        while (_ticks % TicksPerFlush != TicksPerFlush - 1)
            Tick(map);

        Assert.Empty(RemoteCreates(a));
        Assert.Empty(RemoteCreates(b));
        Tick(map);
        Assert.Single(RemoteCreates(a));
    }

    [Fact]
    public void APlayerOutsideTheAdmitRadiusIsNotCreated()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, new Vector3(0, 0, 0));
        FakeClientSession b = Enter(map, 1002, new Vector3(25, 0, 0));

        Flush(map);

        Assert.Empty(RemoteCreates(a));
        Assert.Empty(RemoteCreates(b));
    }

    [Fact]
    public void APlayerBetweenTheRadiiStaysOnceSeen()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, new Vector3(0, 0, 0));
        FakeClientSession b = Enter(map, 1002, new Vector3(10, 0, 0));
        Flush(map);

        Move(map, b, new Vector3(25, 0, 0));
        Flush(map);

        Assert.Empty(a.Sent.OfType<UnitDestroyed>());
        Assert.Empty(b.Sent.OfType<UnitDestroyed>());
    }

    [Fact]
    public void APlayerBeyondTheDropRadiusIsDestroyed()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, new Vector3(0, 0, 0));
        FakeClientSession b = Enter(map, 1002, new Vector3(10, 0, 0));
        Flush(map);

        Move(map, b, new Vector3(31, 0, 0));
        Flush(map);

        Assert.Equal(OwnUnit(b), Assert.Single(a.Sent.OfType<UnitDestroyed>()).UnitId);
        Assert.Equal(OwnUnit(a), Assert.Single(b.Sent.OfType<UnitDestroyed>()).UnitId);
    }

    [Fact]
    public void APlayerNotYetInTheWorldIsNeitherSeenNorSees()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, new Vector3(0, 0, 0));
        FakeClientSession b = Arrive(map, 1002, new Vector3(10, 0, 0));

        Flush(map);

        Assert.Empty(RemoteCreates(a));
        Assert.Empty(RemoteCreates(b));

        map.Post(new ClientWorldReady(b, 3));
        Flush(map);

        Assert.Single(RemoteCreates(a));
        Assert.Single(RemoteCreates(b));
    }

    [Fact]
    public void APlayerThePredicateHidesIsNeverCreatedForThatViewer()
    {
        _hidden.Hide(viewer: 1001, target: 1002);
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, new Vector3(0, 0, 0));
        FakeClientSession b = Enter(map, 1002, new Vector3(10, 0, 0));

        Flush(map);

        Assert.Empty(RemoteCreates(a));
        Assert.Equal(OwnUnit(a), Assert.Single(RemoteCreates(b)).UnitId);
    }

    [Fact]
    public void APlayerThePredicateHidesLaterIsDestroyedForThatViewerOnly()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, new Vector3(0, 0, 0));
        FakeClientSession b = Enter(map, 1002, new Vector3(10, 0, 0));
        Flush(map);

        _hidden.Hide(viewer: 1001, target: 1002);
        Flush(map);

        Assert.Equal(OwnUnit(b), Assert.Single(a.Sent.OfType<UnitDestroyed>()).UnitId);
        Assert.Empty(b.Sent.OfType<UnitDestroyed>());
    }

    [Fact]
    public void TheCapAdmitsTheNearestFirst()
    {
        MapOwner map = NewMap(cap: 1);
        FakeClientSession a = Enter(map, 1001, new Vector3(0, 0, 0));
        FakeClientSession c = Enter(map, 1003, new Vector3(15, 0, 0));
        FakeClientSession b = Enter(map, 1002, new Vector3(5, 0, 0));

        Flush(map);

        Assert.Equal(OwnUnit(b), Assert.Single(RemoteCreates(a)).UnitId);
        Assert.NotEmpty(RemoteCreates(c));
    }

    [Fact]
    public void AdmitsPerCheckAreLimited()
    {
        MapOwner map = NewMap(swaps: 1);
        FakeClientSession a = Enter(map, 1001, new Vector3(0, 0, 0));
        Enter(map, 1002, new Vector3(5, 0, 0));
        Enter(map, 1003, new Vector3(15, 0, 0));

        Flush(map);
        Assert.Single(RemoteCreates(a));

        Flush(map);
        Assert.Equal(2, RemoteCreates(a).Count);
    }

    [Fact]
    public void ALogoutDestroysTheUnitForEveryViewer()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, new Vector3(0, 0, 0));
        FakeClientSession b = Enter(map, 1002, new Vector3(5, 0, 0));
        FakeClientSession c = Enter(map, 1003, new Vector3(10, 0, 0));
        Flush(map);

        map.Post(new ClientLogoutRequested(a, true));
        map.Post(new ClientLogoutConfirmed(a));
        Tick(map);

        Assert.Equal(OwnUnit(a), Assert.Single(b.Sent.OfType<UnitDestroyed>()).UnitId);
        Assert.Equal(OwnUnit(a), Assert.Single(c.Sent.OfType<UnitDestroyed>()).UnitId);
        Flush(map);
        Assert.Single(b.Sent.OfType<UnitDestroyed>());
    }

    [Fact]
    public void AClosedConnectionDestroysTheUnitForEveryViewer()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, new Vector3(0, 0, 0));
        FakeClientSession b = Enter(map, 1002, new Vector3(5, 0, 0));
        FakeClientSession c = Enter(map, 1003, new Vector3(10, 0, 0));
        Flush(map);

        map.Post(new ClientLeft(a));
        Tick(map);

        Assert.Equal(OwnUnit(a), Assert.Single(b.Sent.OfType<UnitDestroyed>()).UnitId);
        Assert.Equal(OwnUnit(a), Assert.Single(c.Sent.OfType<UnitDestroyed>()).UnitId);
    }

    [Fact]
    public void AMapStopDestroysNothingTwiceAndLeavesNoViewer()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, new Vector3(0, 0, 0));
        FakeClientSession b = Enter(map, 1002, new Vector3(5, 0, 0));
        Flush(map);

        Task stopped = map.StopAsync();
        Tick(map);

        Assert.True(stopped.IsCompleted);
        Assert.True(a.Sent.OfType<UnitDestroyed>().Count() <= 1);
        Assert.True(b.Sent.OfType<UnitDestroyed>().Count() <= 1);
        Assert.Equal(2, _realm.Posted.OfType<CharacterReturned>().Count());
        Assert.Equal(0, map.PlayerCount);
    }

    [Fact]
    public void ACreateIsSerializedOncePerTarget()
    {
        MapOwner map = NewMap();
        Enter(map, 1001, new Vector3(0, 0, 0));
        Enter(map, 1002, new Vector3(5, 0, 0));
        Enter(map, 1003, new Vector3(10, 0, 0));

        Flush(map);

        Assert.Equal(3, _serializer.SerializedCount(typeof(UnitCreate)));
        Assert.Equal(3, _serializer.SerializedCount(typeof(UnitPathTypeChanged)));
    }

    [Fact]
    public void ASteadyStateFlushWithTwoPlayersAllocatesNothing()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, new Vector3(0, 0, 0));
        Enter(map, 1002, new Vector3(10, 0, 0));
        for (int i = 0; i < 10; i++)
            Flush(map);
        Assert.Single(RemoteCreates(a));

        for (int flush = 0; flush < 50; flush++)
        {
            for (int i = 0; i < TicksPerFlush; i++)
            {
                Tick(map);
                Assert.Equal(0L, map.LastTickAllocatedBytes);
            }
        }
    }

    private static uint OwnUnit(FakeClientSession session) => session.Single<PlayerUnit>().UnitId;

    private static List<UnitCreate> RemoteCreates(FakeClientSession session) =>
        [.. session.Sent.OfType<UnitCreate>().Where(create => create.UnitId != OwnUnit(session))];

    private static List<UnitPathTypeChanged> RemotePaths(FakeClientSession session) =>
        [.. session.Sent.OfType<UnitPathTypeChanged>().Where(path => path.UnitId != OwnUnit(session))];

    private static int IndexOf(FakeClientSession session, object sent) => session.Sent.ToList().IndexOf(sent);

    private MapOwner NewMap(int cap = 8, int swaps = 8) =>
        new(1, MapOwnerTests.Settings() with { AdmitRadius = 20, DropRadius = 30, VisibleCap = cap, SwapsPerCheck = swaps },
            _persistence, new Random(1), _metrics, _serializer, _hidden, NullLogger<MapOwner>.Instance);

    private void Tick(MapOwner map)
    {
        _now += 25;
        _ticks++;
        map.Tick(_now);
    }

    // Ticks until the next flush has run.
    private void Flush(MapOwner map)
    {
        do
            Tick(map);
        while (_ticks % TicksPerFlush != 0);
    }

    private FakeClientSession Arrive(MapOwner map, ulong id, Vector3 at)
    {
        var session = new FakeClientSession();
        CharacterRecord record = MapOwnerTests.Record(id) with { Name = $"Player {id}", Location = new CharacterLocation(1, 3, at, 0) };
        map.Post(new CharacterArrived(session, 7, record, _realm));
        Tick(map);
        return session;
    }

    private FakeClientSession Enter(MapOwner map, ulong id, Vector3 at)
    {
        FakeClientSession session = Arrive(map, id, at);
        map.Post(new ClientWorldReady(session, 3));
        Tick(map);
        return session;
    }

    private void Move(MapOwner map, FakeClientSession session, Vector3 to)
    {
        map.Post(new ClientControlReceipt(session, session.Single<MovementControl>().Ticket));
        map.Post(new ClientMoved(session, new MovementSample(to, 0), null));
        Tick(map);
    }

    private sealed class HiddenPairs : IVisibilityPredicate
    {
        private readonly HashSet<(ulong Viewer, ulong Target)> _hidden = [];

        public void Hide(ulong viewer, ulong target) => _hidden.Add((viewer, target));

        public bool CanSee(ulong viewerCharacterId, ulong targetCharacterId) => !_hidden.Contains((viewerCharacterId, targetCharacterId));
    }
}
