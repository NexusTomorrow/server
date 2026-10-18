using System.Numerics;
using Microsoft.Extensions.Logging.Abstractions;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Entity.Command;
using NexusTomorrow.Network.World.Entity.Model;
using NexusTomorrow.Network.World.Message.Model.Entity;
using NexusTomorrow.Network.World.Message.Model.Movement;
using NexusTomorrow.Network.World.Message.Model.PlayerPath;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Network.World.Message.Model.Shared;
using NexusTomorrow.Network.World.Message.Model.WorldEntry;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Owners.World;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Realm;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Maps.Tests;

public sealed class WorldEntryTests : IDisposable
{
    private readonly FakeWriteBehind _persistence = new();
    private readonly MapMetrics _metrics = new();
    private readonly FakeRealmInbox _realm = new();
    private long _now;

    public void Dispose() => _metrics.Dispose();

    [Fact]
    public void ArrivalSendsTheEntryInTheClientsOrder()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();

        Arrive(map, session);

        Type[] expected =
        [
            typeof(EnterWorld), typeof(UnitCreate), typeof(PlayerUnit), typeof(WorldSettings), typeof(MovementControl),
            typeof(PlayerState), typeof(PathState),
        ];
        Assert.Equal(expected, [.. session.Sent.Select(sent => sent.GetType())]);
        Assert.Null(session.DisconnectReason);
    }

    [Fact]
    public void EveryEntryMessagePassesTheSendGate()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();

        Arrive(map, session);

        Assert.Equal(7, session.Sent.Count);
        Assert.Null(session.DisconnectReason);
        foreach (object sent in session.Sent)
        {
            var message = (IWireMessage)sent;
            Assert.True(MessageCatalog.Default.MaySend(message, out string? refusal), refusal);
        }
    }

    [Fact]
    public void TheEntryCarriesTheCharacterWhereItStands()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();

        Arrive(map, session);

        EnterWorld enter = session.Single<EnterWorld>();
        Assert.Equal((ushort)1, enter.WorldId);
        Assert.Equal(new Vector3(10, 20, 30), enter.Position);

        UnitCreate create = session.Single<UnitCreate>();
        Assert.Equal((byte)20, create.Unknown1);
        Server0262Union0Arm14 arm = Assert.IsType<Server0262Union0Arm14>(create.Unknown2);
        Assert.Equal(1001UL, arm.Unknown0);
        Assert.Equal((ushort)3, arm.Unknown1);
        Assert.Equal("Rebaser Head", arm.Unknown2);
        Assert.Equal((byte)2, arm.Unknown3);
        Assert.Equal((byte)4, arm.Unknown4);
        Assert.Equal((byte)1, arm.Unknown5);
        Assert.Equal((ushort)166, create.Unknown11);
        Assert.Equal((ushort)166, create.Unknown12);

        Assert.Equal(2, create.Unknown4.Count);
        Assert.Equal((byte)0, create.Unknown4[0].Unknown0);
        Assert.Equal(1000U, StatValue(create.Unknown4[0]));
        Assert.Equal(WorldEntry.PlayerHealth, StatValue(create.Unknown4[0]));
        Assert.Equal((byte)10, create.Unknown4[1].Unknown0);
        Assert.Equal(7U, StatValue(create.Unknown4[1]));

        Assert.Equal(2, create.Unknown6.Count);
        Assert.Equal((byte)2, create.Unknown6[0].Unknown0);
        Assert.Equal(new Vector3(10, 20, 30), Assert.IsType<Server0262Union2Arm02>(create.Unknown6[0].Unknown1).Unknown0);
        Assert.Equal((byte)14, create.Unknown6[1].Unknown0);
        Assert.Equal(new Vector3(1.5f, 0, 0), Assert.IsType<Server0262Union2Arm0E>(create.Unknown6[1].Unknown1).Unknown0);

        Assert.Equal(create.Unknown0, session.Single<PlayerUnit>().UnitId);
        MovementControl control = session.Single<MovementControl>();
        Assert.Equal(create.Unknown0, control.UnitId);
        Assert.True(control.RebuildMovement);
        Assert.Equal(125U, session.Single<WorldSettings>().MovementInterval);

        PlayerState state = session.Single<PlayerState>();
        Assert.Equal(900U, state.Unknown2);
        Assert.Equal(3U, state.Unknown12);
        Assert.Equal(uint.MaxValue, state.Unknown14);

        PathState path = session.Single<PathState>();
        Assert.Equal((byte)2, path.ActivePath);
        Assert.Equal((byte)0b0100, path.UnlockedPaths);
    }

    [Fact]
    public void WorldReadyIsAnsweredOnceWithWorldEntryCompleteAndItsZoneKept()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();
        Arrive(map, session);

        map.Post(new ClientWorldReady(session, 6));
        map.Post(new ClientWorldReady(session, 6));
        Tick(map);

        session.Single<WorldEntryComplete>();
        Assert.Equal((ushort)6, map.PlayerOf(session)!.Location.ZoneId);
    }

    [Fact]
    public void AWorldReadyOfZoneZeroCompletesTheEntryAndKeepsTheZone()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();
        Arrive(map, session);

        map.Post(new ClientWorldReady(session, 0));
        Tick(map);

        session.Single<WorldEntryComplete>();
        Assert.Equal((ushort)3, map.PlayerOf(session)!.Location.ZoneId);
    }

    [Fact]
    public void AZoneChangeKeepsANewZoneAndIgnoresZero()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();
        Arrive(map, session);

        map.Post(new ClientZoneChanged(session, 0));
        Tick(map);
        Assert.Equal((ushort)3, map.PlayerOf(session)!.Location.ZoneId);

        map.Post(new ClientZoneChanged(session, 9));
        Tick(map);
        Assert.Equal((ushort)9, map.PlayerOf(session)!.Location.ZoneId);
    }

    [Fact]
    public void OnlyAReceiptWithTheGrantsTicketGivesControl()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();
        Arrive(map, session);
        uint ticket = session.Single<MovementControl>().Ticket;

        map.Post(new ClientControlReceipt(session, ticket + 1));
        Tick(map);
        Assert.False(map.PlayerOf(session)!.HasControl);

        map.Post(new ClientControlReceipt(session, ticket));
        Tick(map);
        Assert.True(map.PlayerOf(session)!.HasControl);
    }

    [Fact]
    public void TheZoneTheClientReportedGoesBackWithTheCharacter()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();
        Arrive(map, session);

        map.Post(new ClientWorldReady(session, 6));
        map.Post(new ClientLeft(session));
        Tick(map);

        Assert.Equal((ushort)6, Assert.IsType<CharacterReturned>(Assert.Single(_realm.Posted)).Location.ZoneId);
        Assert.Equal((ushort)6, _persistence.Take<SaveCharacterLocationWork>().Location.ZoneId);
    }

    private static uint StatValue(Server0262Part4 stat) => Assert.IsType<Server0262Part4.Server0262Union1Arm00>(stat.Unknown2).Unknown0;

    private MapOwner NewMap() =>
        new(1, MapOwnerTests.Settings(), _persistence, new Random(1), _metrics, NullLogger<MapOwner>.Instance);

    private void Tick(MapOwner map)
    {
        _now += 25;
        map.Tick(_now);
    }

    private void Arrive(MapOwner map, FakeClientSession session)
    {
        map.Post(new CharacterArrived(session, 7, MapOwnerTests.Record(1001), _realm));
        Tick(map);
    }
}
