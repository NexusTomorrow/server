using System.Numerics;
using Microsoft.Extensions.Logging.Abstractions;
using NexusTomorrow.Maps.Visibility;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Entity.Command;
using NexusTomorrow.Network.World.Message.Model.Entity;
using NexusTomorrow.Network.World.Message.Model.Movement;
using NexusTomorrow.Owners.World;
using NexusTomorrow.Persistence.Realm;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Maps.Tests;

public sealed class RelocateTests : IDisposable
{
    private const int TicksPerFlush = 5;

    private static readonly Vector3 s_to = new(5, 6, 7);

    private readonly FakeWriteBehind _persistence = new();
    private readonly MapMetrics _metrics = new();
    private readonly FakeRealmInbox _realm = new();
    private readonly FakeMessageSerializer _serializer = new();
    private long _now;
    private int _ticks;

    public void Dispose() => _metrics.Dispose();

    [Fact]
    public void ARelocateRevokesMovesAndRegrantsInOrder()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        Flush(map);
        Grant(map, a);
        Assert.Equal(1u, a.Single<MovementControl>().Ticket);
        int before = a.Sent.Count;

        Assert.True(Relocate(map, 1001, s_to, 1));

        object[] after = [.. a.Sent.Skip(before)];
        Type[] order = [typeof(MovementControlEnd), typeof(UnitMovement), typeof(MovementControl)];
        Assert.Equal(order, after.Select(message => message.GetType()));
        AssertRelocation((UnitMovement)after[1], OwnUnit(a), (uint)_now);
        var regrant = (MovementControl)after[2];
        Assert.Equal(2u, regrant.Ticket);
        Assert.True(regrant.RebuildMovement);
        Assert.Equal(OwnUnit(a), regrant.UnitId);
    }

    [Fact]
    public void AViewerSeesTheRelocation()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        FakeClientSession b = Enter(map, 1002, new Vector3(10, 0, 0));
        Flush(map);
        Grant(map, a);

        Assert.True(Relocate(map, 1001, s_to, 1));

        AssertRelocation(Assert.Single(b.Sent.OfType<UnitMovement>()), OwnUnit(a), (uint)_now);
        Assert.Equal(1, _serializer.SerializedCount(typeof(UnitMovement)));
    }

    [Fact]
    public void TheNewLocationIsSaved()
    {
        MapOwner map = NewMap(MapOwnerTests.Settings() with { SaveInterval = TimeSpan.FromSeconds(1) });
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        Flush(map);
        Grant(map, a);

        Assert.True(Relocate(map, 1001, s_to, 1));
        for (int i = 0; i < 2 * 1000 / 25; i++)
            Tick(map);

        Assert.Equal(new CharacterLocation(1, 3, s_to, 1), _persistence.Take<SaveCharacterLocationWork>().Location);
    }

    [Fact]
    public void MovementUnderTheOldTicketIsIgnored()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        FakeClientSession b = Enter(map, 1002, new Vector3(10, 0, 0));
        Flush(map);
        Grant(map, a);
        Assert.True(Relocate(map, 1001, s_to, 1));

        // The old ticket's receipt does not restore control, and a batch before the new one's is neither kept nor relayed.
        Grant(map, a, 1);
        Move(map, a, 6000, new Vector3(9, 9, 9));
        Flush(map);

        Assert.Equal(s_to, map.PlayerOf(a)!.Location.Position);
        Assert.Single(b.Sent.OfType<UnitMovement>());
        Assert.Equal(0L, map.RelayedBatches);
    }

    [Fact]
    public void TheFirstMoveAfterTheRegrantResets()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        FakeClientSession b = Enter(map, 1002, new Vector3(10, 0, 0));
        Flush(map);
        Grant(map, a);
        Move(map, a, 5000, Vector3.One);
        Move(map, a, 5125, Vector3.One);
        Flush(map);
        Assert.False(b.Sent.OfType<UnitMovement>().Last().Reset);
        Assert.True(Relocate(map, 1001, s_to, 1));

        Grant(map, a, a.Sent.OfType<MovementControl>().Last().Ticket);
        Move(map, a, 6000, new Vector3(5, 6, 8));
        Flush(map);

        UnitMovement moved = b.Sent.OfType<UnitMovement>().Last();
        Assert.Equal(6000u, moved.Time);
        Assert.True(moved.Reset);
        Assert.False(moved.ServerMoved);
        Assert.Equal(new Vector3(5, 6, 8), map.PlayerOf(a)!.Location.Position);
    }

    [Fact]
    public async Task ABatchQueuedBeforeTheRelocateIsNotRelayedAfterIt()
    {
        MapOwner map = NewMap();
        FakeClientSession a = Enter(map, 1001, Vector3.Zero);
        FakeClientSession b = Enter(map, 1002, new Vector3(10, 0, 0));
        Flush(map);
        Grant(map, a);

        // The batch is handled first in the tick (the high queue), then the relocate; the flush follows.
        Move(map, a, 5000, new Vector3(9, 9, 9));
        Task<bool> relocated = PostRelocate(map, 1001, s_to, 1);
        uint relocatedAt = (uint)(_now + 25);
        Flush(map);

        Assert.True(relocated.IsCompletedSuccessfully);
        Assert.True(await relocated);
        AssertRelocation(Assert.Single(b.Sent.OfType<UnitMovement>()), OwnUnit(a), relocatedAt);
    }

    [Fact]
    public void ARelocateOfACharacterNotInTheWorldAnswersFalse()
    {
        MapOwner map = NewMap();
        var loading = new FakeClientSession();
        map.Post(new CharacterArrived(loading, 7, MapOwnerTests.Record(1001), _realm));
        Tick(map);
        int sent = loading.Sent.Count;

        Assert.False(Relocate(map, 9999, s_to, 1));
        Assert.False(Relocate(map, 1001, s_to, 1));

        Assert.Equal(sent, loading.Sent.Count);
    }

    // The UnitMovement a relocate to s_to facing 1 sends, for the unit, stamped with the map's time at the relocating tick.
    private static void AssertRelocation(UnitMovement moved, uint unitId, uint time)
    {
        Assert.Equal(unitId, moved.UnitId);
        Assert.Equal(time, moved.Time);
        Assert.True(moved.Reset);
        Assert.True(moved.ServerMoved);
        byte[] arms = [2, 14];
        Assert.Equal(arms, moved.Commands.Select(command => command.Unknown0));
        Assert.Equal(s_to, Assert.IsType<SetPositionCommand>(moved.Commands[0].Unknown1).Position);
        Assert.Equal(new Vector3(1, 0, 0), Assert.IsType<SetRotationCommand>(moved.Commands[1].Unknown1).Rotation);
    }

    private static uint OwnUnit(FakeClientSession session) => session.Single<PlayerUnit>().UnitId;

    private MapOwner NewMap() => NewMap(MapOwnerTests.Settings());

    private MapOwner NewMap(MapSettings settings) =>
        new(1, settings with { AdmitRadius = 20, DropRadius = 30 }, _persistence, new Random(1), _metrics, _serializer,
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

    // Posts only, so players entered together are handled in one tick and no flush runs between them.
    private FakeClientSession Enter(MapOwner map, ulong id, Vector3 at)
    {
        var session = new FakeClientSession();
        CharacterRecord record = MapOwnerTests.Record(id) with { Name = $"Player {id}", Location = new CharacterLocation(1, 3, at, 0) };
        map.Post(new CharacterArrived(session, 7, record, _realm));
        map.Post(new ClientWorldReady(session, 3));
        return session;
    }

    private void Grant(MapOwner map, FakeClientSession session) => Grant(map, session, session.Single<MovementControl>().Ticket);

    private void Grant(MapOwner map, FakeClientSession session, uint ticket)
    {
        map.Post(new ClientControlReceipt(session, ticket));
        Tick(map);
    }

    private static void Move(MapOwner map, FakeClientSession session, uint time, Vector3 to)
    {
        Server0262Part5[] commands = [new() { Unknown0 = 2, Unknown1 = new SetPositionCommand { Position = to } }];
        map.PostHigh(new ClientMoved(session, new MovementSample(to, null), new RelayBatch(time, commands)));
    }

    private static Task<bool> PostRelocate(MapOwner map, ulong characterId, Vector3 to, float facing)
    {
        var reply = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        Assert.True(map.Post(new RelocateCharacter(characterId, to, facing, reply)));
        return reply.Task;
    }

    // Posts the relocate and ticks once, so the map has answered.
    private bool Relocate(MapOwner map, ulong characterId, Vector3 to, float facing)
    {
        Task<bool> reply = PostRelocate(map, characterId, to, facing);
        Tick(map);
        Assert.True(reply.IsCompletedSuccessfully);
        return reply.Result;
    }
}
