using NexusTomorrow.Commands;
using NexusTomorrow.Harness;
using NexusTomorrow.Network.Auth.Message.Model;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Network.World.Message.Model.Store;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Realm;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Harness.Tests;

public sealed class PregameScenarios
{
    private const string Email = "alice@example.com";
    private const string Password = "correct horse";

    [Fact]
    public async Task ScriptedLoginReachesAnEmptyCharacterList()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using HeadlessClient client = scenario.Server.NewClient();
        await using WorldSession session = await client.ScriptedLoginAsync(Email, Password);
        Assert.Empty(session.FirstList.Characters);
        Assert.Equal(1, session.FirstList.RealmId);
    }

    [Fact]
    public async Task TheGreetingIsFollowedByEntitlementsAndTheLimitBeforeTheList()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using HeadlessClient client = scenario.Server.NewClient();

        // The login takes the frames after the greeting strictly in order, so a session here received them in this order.
        await using WorldSession session = await client.ScriptedLoginAsync(Email, Password);

        // The harness reads no client tables, so the licenses, which take their counts from the Entitlement table, are not sent.
        uint[] ids = [4, 10, 14, 15, 16, 17, 18, 19, 20, 21, 22, 61, 62];
        Assert.Equal(ids, session.Entitlements.Entitlements.Select(entitlement => entitlement.EntitlementId));
        Assert.Equal((byte)1, session.PremiumTier.Tier);
        Assert.Equal(3u, session.CreationLimit.Level);
        Assert.Empty(session.FirstList.Characters);
    }

    // The account is made as alice@example.com; the login types the email in mixed case at every step.
    [Fact]
    public async Task AMixedCaseEmailLogsInToTheAccount()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using HeadlessClient client = scenario.Server.NewClient();
        await using WorldSession session = await client.ScriptedLoginAsync("Alice@Example.com", Password);
        Assert.Empty(session.FirstList.Characters);
    }

    [Fact]
    public async Task ACreatedCharacterAppearsInTheNextList()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using HeadlessClient client = scenario.Server.NewClient();
        await using WorldSession session = await client.ScriptedLoginAsync(Email, Password);

        CharacterCreateResult created = await session.CreateAsync("Rebaser Head", creationId: 1, path: 2);
        Assert.Equal(CreateResults.Created, created.Result);

        CharacterList list = await session.RequestListAsync();
        CharacterList.Character character = Assert.Single(list.Characters);
        Assert.Equal("Rebaser Head", character.Name);
        Assert.Equal(created.CharacterId, character.CharacterId);
        Assert.Equal(2, character.Unknown15);
        Assert.Equal(166u, character.Unknown5);
    }

    [Fact]
    public async Task ACharacterSurvivesARestart()
    {
        await using PostgresTestDatabase database = await Scenario.DatabaseAsync();
        await using (HarnessServer first = await HarnessServer.StartAsync(database))
        {
            await first.RunAsync($"account create {Email} \"{Password}\"");
            await using HeadlessClient client = first.NewClient();
            await using WorldSession session = await client.ScriptedLoginAsync(Email, Password);
            Assert.Equal(CreateResults.Created, (await session.CreateAsync("Rebaser Head")).Result);
        }

        await using HarnessServer second = await HarnessServer.StartAsync(database);
        await using HeadlessClient again = second.NewClient();
        await using WorldSession later = await again.ScriptedLoginAsync(Email, Password);
        Assert.Equal("Rebaser Head", Assert.Single(later.FirstList.Characters).Name);
    }

    [Fact]
    public async Task TwoAccountsCannotShareANameInAnyCase()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await scenario.Server.RunAsync("account create bob@example.com \"other horse\"");
        await using HeadlessClient alice = scenario.Server.NewClient();
        await using WorldSession aliceSession = await alice.ScriptedLoginAsync(Email, Password);
        Assert.Equal(CreateResults.Created, (await aliceSession.CreateAsync("Rebaser Head")).Result);

        await using HeadlessClient bob = scenario.Server.NewClient();
        await using WorldSession bobSession = await bob.ScriptedLoginAsync("bob@example.com", "other horse");
        Assert.Equal(CreateResults.NameTaken, (await bobSession.CreateAsync("rebaser HEAD")).Result);
    }

    [Fact]
    public async Task AWrongPasswordNeverReachesTheLoginServer()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using HeadlessClient client = scenario.Server.NewClient();
        (Guid? token, StsReply? error) = await client.StsLoginAsync(Email, "wrong");
        Assert.Null(token);
        Assert.Equal("3002", error!.Xml().DocumentElement!.GetAttribute("code"));
    }

    [Fact]
    public async Task AnUnknownAccountLooksLikeAWrongPassword()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using HeadlessClient client = scenario.Server.NewClient();
        (Guid? token, StsReply? error) = await client.StsLoginAsync("nobody@example.com", "anything");
        Assert.Null(token);
        Assert.Equal("3002", error!.Xml().DocumentElement!.GetAttribute("code"));
    }

    [Fact]
    public async Task AGameTokenWorksOnce()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using HeadlessClient client = scenario.Server.NewClient();
        Guid token = (await client.StsLoginAsync(Email, Password)).Token!.Value;
        Assert.NotNull((await client.LoginAsync(token, Email)).Handoff);

        await using HeadlessClient replay = scenario.Server.NewClient();
        Assert.Equal(LoginResults.General, (await replay.LoginAsync(token, Email)).Denied!.Result);
    }

    [Fact]
    public async Task AnExpiredGameTokenIsDenied()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using HeadlessClient client = scenario.Server.NewClient();
        Guid token = (await client.StsLoginAsync(Email, Password)).Token!.Value;
        scenario.Server.Time.Advance(TimeSpan.FromSeconds(61));
        Assert.NotNull((await client.LoginAsync(token, Email)).Denied);
    }

    [Fact]
    public async Task ASecondLoginReplacesTheFirst()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using HeadlessClient first = scenario.Server.NewClient();
        await using WorldSession firstSession = await first.ScriptedLoginAsync(Email, Password);
        await using HeadlessClient second = scenario.Server.NewClient();
        await using WorldSession secondSession = await second.ScriptedLoginAsync(Email, Password);

        Assert.Equal(0, (await firstSession.ExpectAsync<Kicked>()).Unknown0);
        Assert.True(await firstSession.IsClosedAsync(TimeSpan.FromSeconds(10)));
        Assert.Empty((await secondSession.RequestListAsync()).Characters);
    }

    [Fact]
    public async Task ASuspendedAccountIsDenied()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        Assert.Equal(CommandOutcome.Done, (await scenario.Server.RunAsync($"account suspend {Email} forever")).Outcome);
        await using HeadlessClient client = scenario.Server.NewClient();
        Guid token = (await client.StsLoginAsync(Email, Password)).Token!.Value;
        Assert.NotNull((await client.LoginAsync(token, Email)).Denied);
    }

    [Fact]
    public async Task DeleteRemovesACharacterFromTheList()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using HeadlessClient client = scenario.Server.NewClient();
        await using WorldSession session = await client.ScriptedLoginAsync(Email, Password);
        ulong id = (await session.CreateAsync("Rebaser Head")).CharacterId;
        Assert.Equal(DeleteResults.Deleted, (await session.DeleteAsync(id)).Result);
        Assert.Empty((await session.RequestListAsync()).Characters);
    }

    // The storefront request at character select travels inside 0x038C under tag 0x13; it has no row, so it is dropped before its
    // body is read.
    [Fact]
    public async Task AMessageWithNoRowIsDroppedAndTheSessionContinues()
    {
        await using Scenario scenario = await Scenario.StartAsync();
        await using HeadlessClient client = scenario.Server.NewClient();
        await using WorldSession session = await client.ScriptedLoginAsync(Email, Password);
        await session.Connection.SendTaggedAsync(0x13, new Client082D());
        Assert.Empty((await session.RequestListAsync()).Characters);
    }

    // Once a create is queued, it is committed by the time the harness has stopped. The harness drains the owner queues before it stops
    // the host, so this does not show that the host's own stop waits for saves: that order is pinned by
    // HostTests.StoppingWaitsForEveryQueuedSaveAndReportsTheBacklogUntilItCommits in NexusTomorrow.Server.Tests.
    [Fact]
    public async Task ACreatedCharacterIsCommittedOnceTheHarnessHasStopped()
    {
        await using PostgresTestDatabase database = await Scenario.DatabaseAsync();
        await using HarnessServer server = await HarnessServer.StartAsync(database);
        await server.RunAsync($"account create {Email} \"{Password}\"");
        await using HeadlessClient client = server.NewClient();
        await using WorldSession session = await client.ScriptedLoginAsync(Email, Password);
        await session.Connection.SendNestedAsync(new CharacterCreate { CreationId = 1, Name = "Rebaser Head", Path = 0 });
        await Eventually.TrueAsync(() => server.Service<IWriteBehind>().Backlog > 0 || HasCharacter(database));
        await server.DisposeAsync();
        Assert.True(HasCharacter(database));
    }

    private static bool HasCharacter(PostgresTestDatabase database)
    {
        using RealmContext context = database.Contexts.CreateRealm();
        return context.Characters.Any(c => c.Name == "Rebaser Head");
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
