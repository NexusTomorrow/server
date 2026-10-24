using System.Numerics;
using Microsoft.Extensions.Logging.Abstractions;
using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Reward;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Maps.Visibility;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Entity.Command;
using NexusTomorrow.Network.World.Entity.Model;
using NexusTomorrow.Network.World.Message.Model.Entity;
using NexusTomorrow.Network.World.Message.Model.Movement;
using NexusTomorrow.Network.World.Message.Model.PlayerPath;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Network.World.Message.Model.Reward;
using NexusTomorrow.Network.World.Message.Model.Shared;
using NexusTomorrow.Network.World.Message.Model.WorldEntry;
using NexusTomorrow.Owners.Account;
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
    public void EntrySendsThePathAfterThePlayersUnit()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();

        Arrive(map, session);

        Type[] expected =
        [
            typeof(EnterWorld), typeof(UnitCreate), typeof(PlayerUnit), typeof(UnitPathTypeChanged), typeof(WorldSettings),
            typeof(MovementControl), typeof(PlayerState), typeof(PathState),
        ];
        Assert.Equal(expected, [.. session.Sent.Select(sent => sent.GetType())]);
        UnitPathTypeChanged path = session.Single<UnitPathTypeChanged>();
        Assert.Equal(session.Single<UnitCreate>().UnitId, path.UnitId);
        Assert.Equal((byte)2, path.PathType);
        Assert.Null(session.DisconnectReason);
    }

    [Fact]
    public void EveryEntryMessagePassesTheSendGate()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();

        Arrive(map, session);

        Assert.Equal(8, session.Sent.Count);
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
        Assert.Equal(new Vector3(10, 20, 30), Assert.IsType<SetPositionCommand>(create.Commands[0].Unknown1).Position);
        Assert.Equal((byte)14, create.Commands[1].Unknown0);
        Assert.Equal(new Vector3(1.5f, 0, 0), Assert.IsType<SetRotationCommand>(create.Commands[1].Unknown1).Rotation);

        Assert.Equal(create.UnitId, session.Single<PlayerUnit>().UnitId);
        MovementControl control = session.Single<MovementControl>();
        Assert.Equal(create.UnitId, control.UnitId);
        Assert.True(control.RebuildMovement);
        Assert.Equal(125U, session.Single<WorldSettings>().MovementInterval);

        PlayerState state = session.Single<PlayerState>();
        Assert.Equal(900U, state.Experience);
        Assert.Equal(3U, state.InputKeySet);
        Assert.Equal(uint.MaxValue, state.CostumeIndex);

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
        var movement = new ClientMoved(session, new MovementSample(new Vector3(7, 8, 9), 0.5f), null);

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
            map.PostHigh(new ClientMoved(session, new MovementSample(new Vector3(i, 0, 0), i), null));
            Tick(map);
        }

        for (int i = 0; i < 100; i++)
        {
            map.PostHigh(new ClientMoved(session, new MovementSample(new Vector3(i, 1, 0), i), null));
            Tick(map);
            Assert.Equal(0L, map.LastTickAllocatedBytes);
        }

        Assert.Equal(new Vector3(99, 1, 0), map.PlayerOf(session)!.Location.Position);
        Assert.Equal(99f, map.PlayerOf(session)!.Location.Facing);
    }

    [Fact]
    public void ACountdownGoesOutOnlyAnsweringARequest()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();
        Arrive(map, session);
        map.Post(new ClientControlReceipt(session, session.Single<MovementControl>().Ticket));
        map.Post(new ClientWorldReady(session, 6));
        for (int i = 0; i < 30; i++)
            Tick(map);
        Assert.Empty(session.Sent.OfType<LogoutCountdown>());

        map.Post(new ClientLogoutRequested(session, true));
        Tick(map);

        LogoutCountdown countdown = session.Single<LogoutCountdown>();
        Assert.Equal(0U, countdown.Duration);
        Assert.False(countdown.Unknown1);
        Assert.Empty(session.Sent.OfType<WorldLogout>());
        Assert.Equal(1, map.PlayerCount);
        Assert.Empty(_realm.Posted);
    }

    [Fact]
    public void ARequestWithoutBeginSendsNoCountdown()
    {
        MapOwner map = NewMap();
        var session = new FakeClientSession();
        Arrive(map, session);

        map.Post(new ClientLogoutRequested(session, false));
        Tick(map);

        Assert.Empty(session.Sent.OfType<LogoutCountdown>());
        Assert.Equal(1, map.PlayerCount);
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
        Assert.Equal((byte)0, logout.Unknown1);
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

    [Fact]
    public void TheEntryEndsWithTheRewardProperties()
    {
        // Our own fixture values, never the client's: property 60, Discrete, data 0; property 61, AdditiveScalar, data 4.
        MapOwner map = NewMap(MapOwnerTests.Settings() with
        {
            RewardProperties =
            [
                new RewardPropertyValue((RewardPropertyType)60, 0, RewardModifierValueType.Discrete, 0, 7),
                new RewardPropertyValue((RewardPropertyType)61, 4, RewardModifierValueType.AdditiveScalar, 1.25f, 0),
            ],
        });
        var session = new FakeClientSession();

        Arrive(map, session);

        Assert.Equal(9, session.Sent.Count);
        Assert.IsType<PathState>(session.Sent[^2]);
        RewardPropertiesChanged rewards = Assert.IsType<RewardPropertiesChanged>(session.Sent[^1]);
        Assert.True(MessageCatalog.Default.MaySend(rewards, out string? refusal), refusal);
        Assert.Equal(2, rewards.Properties.Count);
        Assert.Equal((RewardPropertyType)60, rewards.Properties[0].Property);
        Assert.Equal(0u, rewards.Properties[0].Data);
        Assert.Equal(RewardModifierValueType.Discrete, rewards.Properties[0].ValueType);
        Assert.Equal(7u, Assert.IsType<RewardPropertiesChanged.Server092CUnion0Arm01>(rewards.Properties[0].Value).Value);
        Assert.Equal((RewardPropertyType)61, rewards.Properties[1].Property);
        Assert.Equal(4u, rewards.Properties[1].Data);
        Assert.Equal(RewardModifierValueType.AdditiveScalar, rewards.Properties[1].ValueType);
        Assert.Equal(1.25f, Assert.IsType<RewardPropertiesChanged.Server092CUnion0Arm00>(rewards.Properties[1].Value).Value);
        Assert.All(rewards.Properties, entry => Assert.Empty(entry.Unknown4));
    }

    [Fact]
    public void EveryPlayerCreateCarriesTheTier()
    {
        MapOwner map = NewMap(MapOwnerTests.Settings() with { PremiumTier = 1 });
        var a = new FakeClientSession();
        var b = new FakeClientSession();
        map.Post(new CharacterArrived(a, 7, MapOwnerTests.Record(1001), _realm));
        map.Post(new ClientWorldReady(a, 3));
        map.Post(new CharacterArrived(b, 7, MapOwnerTests.Record(1002), _realm));
        map.Post(new ClientWorldReady(b, 3));

        // Enough ticks for a replication flush, which creates each player for the other.
        for (int i = 0; i < 5; i++)
            Tick(map);

        UnitCreate[] creates = [.. a.Sent.OfType<UnitCreate>()];
        Assert.Equal(2, creates.Length);
        Assert.All(creates, create => Assert.Equal((byte)1, Assert.IsType<Server0262Union0Arm14>(create.TypeData).PremiumTier));
    }

    private MapOwner NewMap() => NewMap(MapOwnerTests.Settings());

    private MapOwner NewMap(MapSettings settings) =>
        new(1, settings, _persistence, new Random(1), _metrics, new FakeMessageSerializer(), AllowAllVisibility.Instance, NullLogger<MapOwner>.Instance);

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
