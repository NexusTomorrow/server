using System.Numerics;
using NexusTomorrow.Commands;
using NexusTomorrow.Harness;
using NexusTomorrow.Maps;
using NexusTomorrow.Network.World.Entity.Command;
using NexusTomorrow.Network.World.Entity.Model;
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
        uint unitId = world.Unit.Unknown0;
        Assert.Equal(unitId, world.Player.UnitId);
        Assert.Equal(unitId, world.Control.UnitId);
        Assert.Equal(CharacterName, Assert.IsType<Server0262Union0Arm14>(world.Unit.Unknown2).Unknown2);
        Assert.Equal((ushort)166, world.Unit.Unknown12);
        Assert.Equal(HarnessServer.Start, Assert.IsType<Server0262Union2Arm02>(world.Unit.Unknown6[0].Unknown1).Unknown0);
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
        Assert.Equal(stood, Assert.IsType<Server0262Union2Arm02>(world.Unit.Unknown6[0].Unknown1).Unknown0);
        Assert.Equal(new Vector3(1.5f, 0, 0), Assert.IsType<Server0262Union2Arm0E>(world.Unit.Unknown6[1].Unknown1).Unknown0);
    }

    [Fact]
    public async Task ASecondLoginEntersOnceTheFirstHandsTheCharacterBack()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using HeadlessClient first = scenario.Server.NewClient();
        await using WorldSession firstSession = await first.ScriptedLoginAsync(Email, Password);
        ulong id = (await firstSession.CreateAsync(CharacterName)).CharacterId;
        await firstSession.EnterWorldAsync(id);

        await using HeadlessClient second = scenario.Server.NewClient();
        await using WorldSession secondSession = await second.ScriptedLoginAsync(Email, Password);
        EnteredWorld world = await secondSession.EnterWorldAsync(id);

        Assert.Equal((ushort)1, world.Enter.WorldId);
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
        Assert.Equal(SelectReasons.General, (await session.SelectAsync(created.CharacterId)).Reason);
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
        await server.RunAsync($"account create {Email} \"{Password}\"");
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
        await session.EnterWorldAsync((await session.CreateAsync(CharacterName)).CharacterId);
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
        Assert.Equal((byte)0, logout.Reason);
        Assert.Equal((ushort)1, again.Enter.WorldId);
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
