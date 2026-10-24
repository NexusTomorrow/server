using System.Numerics;
using NexusTomorrow.Commands;
using NexusTomorrow.Harness;
using NexusTomorrow.Maps;
using NexusTomorrow.Network.World.Entity.Command;
using NexusTomorrow.Network.World.Entity.Model;
using NexusTomorrow.Network.World.Message.Model.Chat;
using NexusTomorrow.Network.World.Message.Model.Entity;
using NexusTomorrow.Network.World.Message.Model.Movement;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Network.World.Message.Model.WorldEntry;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Realm;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Harness.Tests;

public sealed class WorldScenarios
{
    private const string Email = "alice@example.com";
    private const string Password = "correct horse";
    private const string CharacterName = "Rebaser Head";

    private static readonly TimeSpan s_bound = TimeSpan.FromSeconds(10);

    [Fact]
    public async Task ACreatedCharacterEntersTheWorldInTheClientsOrder()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using HeadlessClient client = scenario.Server.NewClient();
        await using WorldSession session = await client.ScriptedLoginAsync(Email, Password);
        CharacterCreateResult created = await session.CreateAsync(CharacterName, creationId: 1, path: 2);

        EnteredWorld world = await session.EnterWorldAsync(created.CharacterId);

        Assert.Equal((ushort)1, world.Enter.WorldId);
        Assert.Equal(HarnessServer.Start, world.Enter.Position);
        uint unitId = world.Unit.UnitId;
        Assert.Equal(unitId, world.Player.UnitId);
        Assert.Equal(unitId, world.UnitPath.UnitId);
        Assert.Equal((byte)2, world.UnitPath.PathType);
        Assert.Equal(unitId, world.Control.UnitId);
        Assert.Equal(CharacterName, Assert.IsType<Server0262Union0Arm14>(world.Unit.TypeData).Name);
        Assert.Equal((ushort)166, world.Unit.BaseFactionId);
        Assert.Equal(HarnessServer.Start, Assert.IsType<SetPositionCommand>(world.Unit.Commands[0].Unknown1).Position);
        Assert.Equal(125u, world.Settings.MovementInterval);
        Assert.Equal((byte)2, world.Path.ActivePath);
    }

    [Fact]
    public async Task TheZoneTheClientReportsIsSavedWhenItLeaves()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using HeadlessClient client = scenario.Server.NewClient();
        await using WorldSession session = await client.ScriptedLoginAsync(Email, Password);
        ulong id = (await session.CreateAsync(CharacterName)).CharacterId;
        await session.EnterWorldAsync(id, zoneId: 7);

        await session.DisposeAsync();

        await Eventually.TrueAsync(() => scenario.Row(id).ZoneId == 7, s_bound);
    }

    [Fact]
    public async Task ACharacterReentersWhereItStood()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using HeadlessClient client = scenario.Server.NewClient();
        await using WorldSession session = await client.ScriptedLoginAsync(Email, Password);
        ulong id = (await session.CreateAsync(CharacterName)).CharacterId;
        await session.EnterWorldAsync(id);
        var stood = new Vector3(120, 12, -210);
        await session.MoveAsync(stood, 1.5f, 5000);

        await session.DisposeAsync();
        await Eventually.TrueAsync(() => scenario.Row(id).PositionX == 120, s_bound);

        await using HeadlessClient again = scenario.Server.NewClient();
        await using WorldSession later = await again.ScriptedLoginAsync(Email, Password);
        EnteredWorld world = await later.EnterWorldAsync(id);
        Assert.Equal(stood, world.Enter.Position);
        Assert.Equal(stood, Assert.IsType<SetPositionCommand>(world.Unit.Commands[0].Unknown1).Position);
        Assert.Equal(new Vector3(1.5f, 0, 0), Assert.IsType<SetRotationCommand>(world.Unit.Commands[1].Unknown1).Rotation);
    }

    [Fact]
    public async Task ASecondLoginEntersOnceTheFirstHandsTheCharacterBack()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using HeadlessClient first = scenario.Server.NewClient();
        await using WorldSession firstSession = await first.ScriptedLoginAsync(Email, Password);
        ulong id = (await firstSession.CreateAsync(CharacterName)).CharacterId;
        await firstSession.EnterWorldAsync(id);
        var moved = new Vector3(140, 16, -230);
        await firstSession.MoveAsync(moved, 0.75f, 5000);

        // The list answers after the movement was read, so the movement is in the map's queue by then; the pause waits out the step
        // that applies it.
        await firstSession.RequestListAsync();
        MapOwner map = scenario.Server.Map(1);
        await Eventually.TrueAsync(() => map.QueueLength == 0, s_bound);
        await scenario.Server.Maps.PauseAsync();

        await using HeadlessClient second = scenario.Server.NewClient();
        await using WorldSession secondSession = await second.ScriptedLoginAsync(Email, Password);
        Task<EnteredWorld> entering = secondSession.EnterWorldAsync(id);

        // The second login ended the first session, but a paused map has not handled its leaving and still holds the character, so
        // the select waits for the hand-back.
        await Task.Delay(TimeSpan.FromMilliseconds(500), TestContext.Current.CancellationToken);
        Assert.False(entering.IsCompleted);
        Assert.Equal(1, map.PlayerCount);

        scenario.Server.Maps.Resume();
        EnteredWorld world = await entering;

        Assert.Equal(moved, world.Enter.Position);
        Assert.Equal(moved, Assert.IsType<SetPositionCommand>(world.Unit.Commands[0].Unknown1).Position);
        Assert.True(await firstSession.IsClosedAsync(s_bound));
    }

    [Fact]
    public async Task ACharacterWhoseWorldIsHeldCannotEnter()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using HeadlessClient client = scenario.Server.NewClient();
        await using WorldSession session = await client.ScriptedLoginAsync(Email, Password);
        CharacterCreateResult created = await session.CreateAsync(CharacterName, creationId: 3);
        Assert.Equal(CreateResults.Created, created.Result);

        // An unknown id is refused with the same reason, so the created character is what makes this the held world's refusal. No
        // map was made for either world.
        Assert.Equal(SelectReasons.General, (await session.SelectAsync(created.CharacterId)).Unknown0);
        Assert.Empty(scenario.Server.Service<MapDirectory>().Maps);
    }

    [Fact]
    public async Task AWorldMessageUnderAnotherTagEndsTheSession()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using HeadlessClient client = scenario.Server.NewClient();
        await using WorldSession session = await client.ScriptedLoginAsync(Email, Password);
        await session.EnterWorldAsync((await session.CreateAsync(CharacterName)).CharacterId);

        await session.Connection.SendTaggedAsync(0x0B, new ZoneChange());

        Assert.True(await session.IsClosedAsync(s_bound));
    }

    [Fact]
    public async Task StoppingTheServerSavesWhereTheCharacterStood()
    {
        await using PostgresTestDatabase database = await Scenario.DatabaseAsync();
        await using HarnessServer server = await HarnessServer.StartAsync(database);
        Assert.Equal(CommandOutcome.Done, (await server.RunAsync($"account create {Email} \"{Password}\"")).Outcome);
        await using HeadlessClient client = server.NewClient();
        await using WorldSession session = await client.ScriptedLoginAsync(Email, Password);
        ulong id = (await session.CreateAsync(CharacterName)).CharacterId;
        await session.EnterWorldAsync(id, zoneId: 7);
        await session.MoveAsync(new Vector3(130, 14, -220), 0.5f, 5000);
        await session.RequestListAsync();
        await Eventually.TrueAsync(() => server.Map(1).QueueLength == 0, s_bound);

        await server.DisposeAsync();

        CharacterRow row = Scenario.Row(database, id);
        Assert.Equal((ushort)7, row.ZoneId);
        Assert.Equal(130f, row.PositionX);
        Assert.Equal(0.5f, row.Facing);
    }

    [Fact]
    public async Task ASteadyStateMapTickAllocatesNothing()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using HeadlessClient client = scenario.Server.NewClient();
        await using WorldSession session = await client.ScriptedLoginAsync(Email, Password);
        await session.EnterWorldAsync((await session.CreateAsync(CharacterName)).CharacterId);
        MapOwner map = scenario.Server.Map(1);
        await scenario.Server.Maps.PauseAsync();
        await scenario.Server.Maps.TickAsync(20);

        for (int i = 0; i < 100; i++)
        {
            await scenario.Server.Maps.TickAsync();
            Assert.Equal(0L, map.LastTickAllocatedBytes);
        }
    }

    [Fact]
    public async Task ATickThatAppliesTheClientsMovementAllocatesNothing()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using HeadlessClient client = scenario.Server.NewClient();
        await using WorldSession session = await client.ScriptedLoginAsync(Email, Password);
        ulong id = (await session.CreateAsync(CharacterName)).CharacterId;
        await session.EnterWorldAsync(id);
        MapOwner map = scenario.Server.Map(1);
        await scenario.Server.Maps.PauseAsync();

        for (int i = 0; i < 25; i++)
        {
            await session.MoveAsync(new Vector3(100 + i, 10, -200), 0, (uint)(6000 + 125 * i));
            await Eventually.TrueAsync(() => map.QueueLength == 1, s_bound);
            await scenario.Server.Maps.TickAsync();
            if (i >= 5)
                Assert.Equal(0L, map.LastTickAllocatedBytes);
        }

        // The hand-back saves where the character stood, so the row holds the last movement the map applied.
        scenario.Server.Maps.Resume();
        await session.DisposeAsync();
        await Eventually.TrueAsync(() => scenario.Row(id).PositionX == 124, s_bound);
        CharacterRow row = scenario.Row(id);
        Assert.Equal(new Vector3(124, 10, -200), new Vector3(row.PositionX, row.PositionY, row.PositionZ));
    }

    [Fact]
    public async Task ALoggedOutCharacterCanBeSelectedAgain()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using HeadlessClient client = scenario.Server.NewClient();
        await using WorldSession session = await client.ScriptedLoginAsync(Email, Password);
        ulong id = (await session.CreateAsync(CharacterName)).CharacterId;
        await session.EnterWorldAsync(id);

        (LogoutCountdown countdown, WorldLogout logout) = await session.LogOutAsync();
        // A select is rate limited to one a second on the host's clock, which is the fake one here and refills only when it moves.
        scenario.Server.Time.Advance(TimeSpan.FromSeconds(2));
        EnteredWorld again = await session.EnterWorldAsync(id);

        Assert.Equal(0u, countdown.Duration);
        Assert.True(logout.ToCharacterSelect);
        Assert.Equal((byte)0, logout.Unknown1);
        Assert.Equal((ushort)1, again.Enter.WorldId);
    }

    [Fact]
    public async Task TwoPlayersEnteringTheSameWorldSeeEachOther()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using Walkers walkers = await Walkers.EnterAsync(scenario);

        Assert.Equal(walkers.SecondUnit, walkers.FirstSaw.UnitId);
        Assert.Equal((byte)20, walkers.FirstSaw.UnitType);
        Assert.Equal(Walkers.SecondName, Assert.IsType<Server0262Union0Arm14>(walkers.FirstSaw.TypeData).Name);
        Assert.Equal(walkers.SecondUnit, walkers.FirstSawPath.UnitId);
        Assert.Equal(walkers.FirstUnit, walkers.SecondSaw.UnitId);
        Assert.Equal(CharacterName, Assert.IsType<Server0262Union0Arm14>(walkers.SecondSaw.TypeData).Name);
        Assert.Equal(walkers.FirstUnit, walkers.SecondSawPath.UnitId);
    }

    [Fact]
    public async Task AMoveReachesTheOtherPlayer()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using Walkers walkers = await Walkers.EnterAsync(scenario);
        Vector3 to = HarnessServer.Start + new Vector3(2, 0, 0);

        await walkers.First.MoveAsync(to, 0.5f, 7000);

        UnitMovement moved = await walkers.Second.ExpectAsync<UnitMovement>();
        Assert.Equal(walkers.FirstUnit, moved.UnitId);
        Assert.Equal(7000u, moved.Time);
        Assert.True(moved.Reset);
        byte[] arms = [0, 2, 14];
        Assert.Equal(arms, moved.Commands.Select(command => command.Unknown0));
        Assert.Equal(to, Assert.IsType<SetPositionCommand>(moved.Commands[1].Unknown1).Position);
        Assert.Equal(new Vector3(0.5f, 0, 0), Assert.IsType<SetRotationCommand>(moved.Commands[2].Unknown1).Rotation);
    }

    [Fact]
    public async Task ASayLineReachesThePlayerInRangeAndIsAcceptedForTheSpeaker()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using Walkers walkers = await Walkers.EnterAsync(scenario);

        await walkers.First.SayAsync("hello", 1);

        ChatSendAccepted accepted = await walkers.First.ExpectAsync<ChatSendAccepted>();
        Assert.Equal((ushort)1, accepted.ChatId);
        Assert.Equal(CharacterName, accepted.SenderName);
        ChatMessageReceived heard = await walkers.Second.ExpectAsync<ChatMessageReceived>();
        Assert.Equal((ushort)4, heard.Channel.Type);
        Assert.Equal(CharacterName, heard.SenderName);
        Assert.Equal("hello", heard.Text);
        Assert.Equal((ulong)walkers.FirstUnit, heard.UnitId);
    }

    [Fact]
    public async Task ALogoutRemovesThePlayerFromTheOthersWorld()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using Walkers walkers = await Walkers.EnterAsync(scenario);

        await walkers.First.LogOutAsync();

        Assert.Equal(walkers.FirstUnit, (await walkers.Second.ExpectAsync<UnitDestroyed>()).UnitId);
    }

    [Fact]
    public async Task WalkingOutOfRangeRemovesAndWalkingBackRestores()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using Walkers walkers = await Walkers.EnterAsync(scenario);

        // The drop radius is 160 and the admit radius 128 (the Maps defaults).
        await walkers.First.MoveAsync(HarnessServer.Start + new Vector3(200, 0, 0), 0, 7000);
        Assert.Equal(walkers.FirstUnit, (await walkers.Second.ExpectAsync<UnitDestroyed>()).UnitId);

        await walkers.First.MoveAsync(HarnessServer.Start + new Vector3(50, 0, 0), 0, 7125);
        Assert.Equal(walkers.FirstUnit, (await walkers.Second.ExpectAsync<UnitCreate>()).UnitId);
        Assert.Equal(walkers.FirstUnit, (await walkers.Second.ExpectAsync<UnitPathTypeChanged>()).UnitId);
        UnitMovement moved = await walkers.Second.ExpectAsync<UnitMovement>();
        Assert.Equal(7125u, moved.Time);
        Assert.True(moved.Reset);
    }

    [Fact]
    public async Task ASteadyStateTickWithTwoPlayersMovingAllocatesNothing()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using Walkers walkers = await Walkers.EnterAsync(scenario);
        MapOwner map = scenario.Server.Map(1);
        await scenario.Server.Maps.PauseAsync();

        // Ten seconds of 125 ms intervals; the first ten warm the pools and the queues. Each moves the host's clock too, since the
        // movement row's rate limit refills on it, and it is the fake one here.
        uint clock = 7000;
        for (int interval = 0; interval < 80; interval++)
        {
            clock += 125;
            scenario.Server.Time.Advance(TimeSpan.FromMilliseconds(125));
            await walkers.First.MoveAsync(HarnessServer.Start + new Vector3(interval % 5, 0, 0), 0, clock);
            await walkers.Second.MoveAsync(HarnessServer.Start + new Vector3(0, 0, interval % 5), 0, clock);
            await Eventually.TrueAsync(() => map.QueueLength == 2, s_bound);
            for (int tick = 0; tick < 5; tick++)
            {
                await scenario.Server.Maps.TickAsync();
                if (interval >= 10)
                    Assert.Equal(0L, map.LastTickAllocatedBytes);
            }
        }

        Assert.True(map.RelayedBatches >= 160);
    }

    // Two accounts, each with a character entered at the start: a second character on one account would replace the first's login.
    // Each has taken the other's create and path.
    private sealed class Walkers : IAsyncDisposable
    {
        public const string SecondName = "Other Walker";
        private const string SecondEmail = "bob@example.com";

        private HeadlessClient? _firstClient;
        private HeadlessClient? _secondClient;

        public WorldSession First { get; private set; } = null!;

        public WorldSession Second { get; private set; } = null!;

        public uint FirstUnit { get; private set; }

        public uint SecondUnit { get; private set; }

        public UnitCreate FirstSaw { get; private set; } = null!;

        public UnitPathTypeChanged FirstSawPath { get; private set; } = null!;

        public UnitCreate SecondSaw { get; private set; } = null!;

        public UnitPathTypeChanged SecondSawPath { get; private set; } = null!;

        public static async Task<Walkers> EnterAsync(Scenario scenario)
        {
            Assert.Equal(CommandOutcome.Done, (await scenario.Server.RunAsync($"account create {SecondEmail} \"{Password}\"")).Outcome);
            var walkers = new Walkers { _firstClient = scenario.Server.NewClient(), _secondClient = scenario.Server.NewClient() };
            walkers.First = await walkers._firstClient.ScriptedLoginAsync(Email, Password);
            walkers.FirstUnit = (await walkers.First.EnterWorldAsync((await walkers.First.CreateAsync(CharacterName)).CharacterId)).Unit.UnitId;
            walkers.Second = await walkers._secondClient.ScriptedLoginAsync(SecondEmail, Password);
            walkers.SecondUnit = (await walkers.Second.EnterWorldAsync((await walkers.Second.CreateAsync(SecondName)).CharacterId)).Unit.UnitId;
            walkers.FirstSaw = await walkers.First.ExpectAsync<UnitCreate>();
            walkers.FirstSawPath = await walkers.First.ExpectAsync<UnitPathTypeChanged>();
            walkers.SecondSaw = await walkers.Second.ExpectAsync<UnitCreate>();
            walkers.SecondSawPath = await walkers.Second.ExpectAsync<UnitPathTypeChanged>();
            return walkers;
        }

        public async ValueTask DisposeAsync()
        {
            await First.DisposeAsync();
            await Second.DisposeAsync();
            if (_firstClient is not null)
                await _firstClient.DisposeAsync();
            if (_secondClient is not null)
                await _secondClient.DisposeAsync();
        }
    }

    private sealed class Scenario : IAsyncDisposable
    {
        private Scenario(PostgresTestDatabase database, HarnessServer server)
        {
            Database = database;
            Server = server;
        }

        public PostgresTestDatabase Database { get; }

        public HarnessServer Server { get; }

        public static async Task<PostgresTestDatabase> DatabaseAsync()
        {
            PostgresTestDatabase? database = await PostgresTestDatabase.CreateAsync();
            if (database is null)
                Assert.Skip($"No PostgreSQL: set {PostgresTestDatabase.ConnectionVariable}.");
            return database;
        }

        // The database and the server are released if the account cannot be made, so a failed start leaves nothing for the next test.
        public static async Task<Scenario> StartAsync()
        {
            PostgresTestDatabase database = await DatabaseAsync();
            HarnessServer? server = null;
            try
            {
                server = await HarnessServer.StartAsync(database);
                Assert.Equal(CommandOutcome.Done, (await server.RunAsync($"account create {Email} \"{Password}\"")).Outcome);
                return new Scenario(database, server);
            }
            catch
            {
                if (server is not null)
                    await server.DisposeAsync();
                await database.DisposeAsync();
                throw;
            }
        }

        // The character's row, read fresh.
        public static CharacterRow Row(PostgresTestDatabase database, ulong id)
        {
            using RealmContext context = database.Contexts.CreateRealm();
            return context.Characters.Single(c => c.Id == id);
        }

        public CharacterRow Row(ulong id) => Row(Database, id);

        public async ValueTask DisposeAsync()
        {
            try
            {
                await Server.DisposeAsync();
            }
            finally
            {
                await Database.DisposeAsync();
            }
        }
    }
}
