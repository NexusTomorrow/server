using System.Numerics;
using Microsoft.Extensions.Logging.Abstractions;
using NexusTomorrow.Game.Static.Entity;
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
        Assert.Equal((byte)20, create.UnitType);
        Server0262Union0Arm14 arm = Assert.IsType<Server0262Union0Arm14>(create.TypeData);
        Assert.Equal(1001UL, arm.CharacterId);
        Assert.Equal((ushort)11, arm.RealmId);
        Assert.Equal("Rebaser Head", arm.Name);
        Assert.Equal((byte)2, arm.RaceId);
        Assert.Equal((byte)4, arm.ClassId);
        Assert.Equal(Sex.Female, arm.Gender);
        Assert.Equal((ushort)166, create.FactionId);
        Assert.Equal((ushort)166, create.BaseFactionId);

        Assert.Equal(2, create.Stats.Count);
        Assert.Equal((byte)0, create.Stats[0].Unknown0);
        Assert.Equal(1000U, StatValue(create.Stats[0]));
        Assert.Equal(WorldEntry.PlayerHealth, StatValue(create.Stats[0]));
        Assert.Equal((byte)10, create.Stats[1].Unknown0);
        Assert.Equal(7U, StatValue(create.Stats[1]));

        Assert.Equal(2, create.Commands.Count);
        Assert.Equal((byte)2, create.Commands[0].Unknown0);
        Assert.Equal(new Vector3(10, 20, 30), Assert.IsType<Server0262Union2Arm02>(create.Commands[0].Unknown1).Unknown0);
        Assert.Equal((byte)14, create.Commands[1].Unknown0);
        Assert.Equal(new Vector3(1.5f, 0, 0), Assert.IsType<Server0262Union2Arm0E>(create.Commands[1].Unknown1).Unknown0);

        Assert.Equal(create.UnitId, session.Single<PlayerUnit>().UnitId);
        MovementControl control = session.Single<MovementControl>();
        Assert.Equal(create.UnitId, control.UnitId);
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

    [Fact]
    public void MovementIsKeptOnceTheClientHasControl()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();
        Arrive(map, session);
        uint ticket = session.Single<MovementControl>().Ticket;
        var movement = new ClientMoved(session, new MovementSample(new Vector3(7, 8, 9), 0.5f));

        map.PostHigh(movement);
        Tick(map);
        Assert.Equal(new Vector3(10, 20, 30), map.PlayerOf(session)!.Location.Position);

        map.Post(new ClientControlReceipt(session, ticket));
        Tick(map);
        map.PostHigh(movement);
        Tick(map);

        Assert.Equal(new Vector3(7, 8, 9), map.PlayerOf(session)!.Location.Position);
        Assert.Equal(0.5f, map.PlayerOf(session)!.Location.Facing);
    }

    [Fact]
    public void ATickThatAppliesMovementAllocatesNothing()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();
        Arrive(map, session);
        map.Post(new ClientControlReceipt(session, session.Single<MovementControl>().Ticket));
        Tick(map);

        for (int i = 0; i < 20; i++)
        {
            map.PostHigh(new ClientMoved(session, new MovementSample(new Vector3(i, 0, 0), i)));
            Tick(map);
        }

        for (int i = 0; i < 100; i++)
        {
            map.PostHigh(new ClientMoved(session, new MovementSample(new Vector3(i, 1, 0), i)));
            Tick(map);
            Assert.Equal(0L, map.LastTickAllocatedBytes);
        }

        Assert.Equal(new Vector3(99, 1, 0), map.PlayerOf(session)!.Location.Position);
        Assert.Equal(99f, map.PlayerOf(session)!.Location.Facing);
    }

    [Fact]
    public void ALogoutRequestIsAnsweredWithACountdownOfZero()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();
        Arrive(map, session);

        map.Post(new ClientLogoutRequested(session, true));
        Tick(map);

        LogoutCountdown countdown = session.Single<LogoutCountdown>();
        Assert.Equal(0U, countdown.Duration);
        Assert.False(countdown.Cancel);
        Assert.Empty(session.Sent.OfType<WorldLogout>());
        Assert.Equal(1, map.PlayerCount);
        Assert.Empty(_realm.Posted);
    }

    [Fact]
    public void AConfirmedLogoutHandsTheCharacterBackThenReturnsTheClientToCharacterSelect()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();
        var moved = new CharacterLocation(1, 5, new Vector3(4, 5, 6), 1f);
        int savesWhenReturned = -1;
        int logoutsWhenReturned = -1;
        _realm.OnPost = _ =>
        {
            savesWhenReturned = _persistence.Backlog;
            logoutsWhenReturned = session.Sent.OfType<WorldLogout>().Count();
        };
        Arrive(map, session);
        map.PlayerOf(session)!.MoveTo(moved);
        map.Post(new ClientLogoutRequested(session, true));
        Tick(map);

        map.Post(new ClientLogoutConfirmed(session));
        Tick(map);

        Assert.Equal(1, savesWhenReturned);
        Assert.Equal(0, logoutsWhenReturned);
        Assert.Equal(moved, _persistence.Take<SaveCharacterLocationWork>().Location);
        Assert.Equal(moved, Assert.IsType<CharacterReturned>(Assert.Single(_realm.Posted)).Location);
        WorldLogout logout = session.Single<WorldLogout>();
        Assert.True(logout.ToCharacterSelect);
        Assert.Equal((byte)0, logout.Reason);
        Assert.Null(session.Map);
        Assert.Null(session.DisconnectReason);
        Assert.Equal(0, map.PlayerCount);
    }

    [Fact]
    public void ALogoutConfirmWithNoRequestPendingIsDroppedAndCounted()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();
        Arrive(map, session);

        map.Post(new ClientLogoutConfirmed(session));
        Tick(map);

        Assert.Empty(session.Sent.OfType<WorldLogout>());
        Assert.Equal(1, map.PlayerCount);
        Assert.Empty(_realm.Posted);
        Assert.Equal(1L, map.DroppedMessages);
        Assert.Null(session.DisconnectReason);
    }

    [Fact]
    public void ACancelledLogoutRequestKeepsTheCharacterInTheWorld()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();
        Arrive(map, session);
        map.Post(new ClientLogoutRequested(session, true));
        Tick(map);

        map.Post(new ClientLogoutRequested(session, false));
        Tick(map);
        map.Post(new ClientLogoutConfirmed(session));
        Tick(map);

        session.Single<LogoutCountdown>();
        Assert.Empty(session.Sent.OfType<WorldLogout>());
        Assert.Equal(1, map.PlayerCount);
        Assert.Equal(1L, map.DroppedMessages);
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
