using System.Collections.Immutable;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Game.Static;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Realm;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Owners.Tests;

public sealed class RealmOwnerTests : IAsyncLifetime
{
    private const long Account = 7;

    // Made-up creation options: our own fixture values, not table rows.
    private static readonly FixedCreationCatalog s_catalog = new(
    [
        new CreationOption(1, Race: 1, Sex: 0, Class: 1, FactionId: 166, Enabled: true, CharacterCreationStart.Arkship, Xp: 0, Level: 1, EntitlementRequired: 0, CostAmount: 0, CostumeOnly: false),
        new CreationOption(2, Race: 1, Sex: 0, Class: 1, FactionId: 166, Enabled: false, CharacterCreationStart.Arkship, Xp: 0, Level: 1, EntitlementRequired: 0, CostAmount: 0, CostumeOnly: false),
        new CreationOption(3, Race: 1, Sex: 0, Class: 1, FactionId: 166, Enabled: true, CharacterCreationStart.Level50, Xp: 0, Level: 50, EntitlementRequired: 0, CostAmount: 1, CostumeOnly: false),
    ]);

    private readonly FakeTimeProvider _time = new(new DateTimeOffset(2026, 10, 16, 12, 0, 0, TimeSpan.Zero));
    private readonly FakeWriteBehind _db = new();
    private readonly FakeClientSession _session = new();
    private readonly RealmOwner _owner;

    public RealmOwnerTests()
    {
        _owner = new RealmOwner(_db, s_catalog, _time, Options.Create(new RealmOptions { MaxCharactersPerAccount = 2 }), NullLogger<RealmOwner>.Instance);
    }

    public ValueTask InitializeAsync()
    {
        _owner.Post(new IdBlockTaken(new IdBlock(1001, 1000)));
        _owner.Start();
        return ValueTask.CompletedTask;
    }

    public async ValueTask DisposeAsync() => await _owner.StopAsync().WaitAsync(TimeSpan.FromSeconds(10));

    [Fact]
    public async Task AListLoadsOnceAndAnswersFromTheCache()
    {
        await ListAsync(Summary(5, "Rebaser Head"));
        CharacterList list = _session.Single<CharacterList>();
        Assert.Equal("Rebaser Head", Assert.Single(list.Characters).Name);
        Assert.Equal((ulong)_time.GetUtcNow().ToFileTime(), list.ServerTime);

        _owner.Post(new ListCharacters(Account, _session));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterList>().Count() == 2);
        Assert.Empty(_db.Queued.OfType<LoadCharactersWork>());
    }

    [Fact]
    public async Task ACreateIsAnsweredAfterItsCommitWithResult3()
    {
        await ListAsync();
        _owner.Post(new CreateCharacter(Account, _session, Create("Rebaser Head")));
        await Eventually.TrueAsync(() => _db.Queued.OfType<CreateCharacterWork>().Any());
        Assert.Empty(_session.Sent.OfType<CharacterCreateResult>());

        _db.Take<CreateCharacterWork>().Complete(WorkResult.Committed);
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterCreateResult>().Any());
        CharacterCreateResult result = _session.Single<CharacterCreateResult>();
        Assert.Equal(CreateResults.Created, result.Result);
        Assert.Equal(1001UL, result.CharacterId);

        _owner.Post(new ListCharacters(Account, _session));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterList>().Count() == 2);
        Assert.Equal("Rebaser Head", Assert.Single(_session.Sent.OfType<CharacterList>().Last().Characters).Name);
    }

    [Fact]
    public async Task ANameTheDatabaseRefusesAnswersResult6()
    {
        await ListAsync();
        _owner.Post(new CreateCharacter(Account, _session, Create("Rebaser Head")));
        await Eventually.TrueAsync(() => _db.Queued.OfType<CreateCharacterWork>().Any());
        _db.Take<CreateCharacterWork>().Complete(WorkResult.RefusedBy(RealmContext.LiveNameIndex));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterCreateResult>().Any());
        Assert.Equal(CreateResults.NameTaken, _session.Single<CharacterCreateResult>().Result);
    }

    [Fact]
    public async Task ANamePendingInAnyCaseAnswersResult6()
    {
        var other = new FakeClientSession();
        await ListAsync();
        await ListAsync(other, accountId: 8);
        _owner.Post(new CreateCharacter(Account, _session, Create("Rebaser Head")));
        _owner.Post(new CreateCharacter(8, other, Create("rebaser HEAD")));
        await Eventually.TrueAsync(() => other.Sent.OfType<CharacterCreateResult>().Any());
        Assert.Equal(CreateResults.NameTaken, other.Single<CharacterCreateResult>().Result);
        Assert.Single(_db.Queued.OfType<CreateCharacterWork>());
    }

    [Theory]
    [InlineData("Rebaserhead", 10)]
    [InlineData("Rebaser H3ad", 10)]
    public async Task ANameOutsideTheRulesAnswersResult10(string name, int expected)
    {
        await ListAsync();
        _owner.Post(new CreateCharacter(Account, _session, Create(name)));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterCreateResult>().Any());
        Assert.Equal(expected, _session.Single<CharacterCreateResult>().Result);
        Assert.Empty(_db.Queued.OfType<CreateCharacterWork>());
    }

    [Fact]
    public async Task AFullAccountAnswersResult9()
    {
        await ListAsync(Summary(5, "Aaaa Bbbb"), Summary(6, "Cccc Dddd"));
        _owner.Post(new CreateCharacter(Account, _session, Create("Eeee Ffff")));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterCreateResult>().Any());
        Assert.Equal(CreateResults.AccountFull, _session.Single<CharacterCreateResult>().Result);
    }

    [Theory]
    [InlineData(2u)]  // switched off
    [InlineData(3u)]  // a level-50 start with a cost
    [InlineData(99u)] // not in the catalog
    public async Task AnOptionNotOfferedAnswersTheGeneralFailure(uint creationId)
    {
        await ListAsync();
        CharacterCreate request = Create("Rebaser Head");
        request.CreationId = creationId;
        _owner.Post(new CreateCharacter(Account, _session, request));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterCreateResult>().Any());
        Assert.Equal(CreateResults.General, _session.Single<CharacterCreateResult>().Result);
    }

    [Fact]
    public async Task ADeleteIsAnsweredAfterItsCommitAndTheListForgetsIt()
    {
        await ListAsync(Summary(5, "Rebaser Head"));
        _owner.Post(new DeleteCharacter(Account, _session, 5));
        await Eventually.TrueAsync(() => _db.Queued.OfType<DeleteCharacterWork>().Any());
        Assert.Empty(_session.Sent.OfType<CharacterDeleteResult>());

        _db.Take<DeleteCharacterWork>().Complete(WorkResult.Committed);
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterDeleteResult>().Any());
        Assert.Equal(DeleteResults.Deleted, _session.Single<CharacterDeleteResult>().Result);

        _owner.Post(new ListCharacters(Account, _session));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterList>().Count() == 2);
        Assert.Empty(_session.Sent.OfType<CharacterList>().Last().Characters);
    }

    [Fact]
    public async Task DeletingAnotherAccountsCharacterAnswersTheGeneralFailure()
    {
        await ListAsync();
        _owner.Post(new DeleteCharacter(Account, _session, 5));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterDeleteResult>().Any());
        Assert.Equal(DeleteResults.General, _session.Single<CharacterDeleteResult>().Result);
    }

    [Fact]
    public async Task ARenameNeedsTheRenameMark()
    {
        await ListAsync(Summary(5, "Rebaser Head"));
        _owner.Post(new RenameCharacter(Account, _session, 5, "Other Name"));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterRenameResult>().Any());
        Assert.Equal(RenameResults.NoRenameAvailable, _session.Single<CharacterRenameResult>().Result);
    }

    [Fact]
    public async Task AMarkedCharacterIsRenamedAfterTheCommit()
    {
        await ListAsync(Summary(5, "Rebaser Head") with { RequiresRename = true });
        _owner.Post(new RenameCharacter(Account, _session, 5, "Other Name"));
        await Eventually.TrueAsync(() => _db.Queued.OfType<RenameCharacterWork>().Any());
        Assert.Empty(_session.Sent.OfType<CharacterRenameResult>());

        _db.Take<RenameCharacterWork>().Complete(WorkResult.Committed);
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterRenameResult>().Any());
        CharacterRenameResult result = _session.Single<CharacterRenameResult>();
        Assert.Equal(RenameResults.Renamed, result.Result);
        Assert.Equal("Other Name", result.Name);
        Assert.Equal(5UL, result.CharacterId);
    }

    [Theory]
    [InlineData(false, false, 2)]
    [InlineData(true, false, 1)]
    [InlineData(false, true, 3)]
    public async Task ASelectionAnswersSelectFailedWithItsReason(bool requiresRename, bool disabled, int reason)
    {
        await ListAsync(Summary(5, "Rebaser Head") with { RequiresRename = requiresRename, Disabled = disabled });
        _owner.Post(new SelectCharacter(Account, _session, 5));
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterSelectFailed>().Any());
        Assert.Equal(reason, _session.Single<CharacterSelectFailed>().Reason);
    }

    private Task ListAsync(params CharacterSummary[] characters) => ListAsync(_session, Account, characters);

    private async Task ListAsync(FakeClientSession session, long accountId, params CharacterSummary[] characters)
    {
        _owner.Post(new ListCharacters(accountId, session));
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharactersWork>().Any(w => w.Keys[0] == PersistKey.Account(accountId)));
        LoadCharactersWork load = _db.Queued.OfType<LoadCharactersWork>().First(w => w.Keys[0] == PersistKey.Account(accountId));
        _db.Remove(load);
        load.Characters = [.. characters];
        load.Complete(WorkResult.Committed);
        await Eventually.TrueAsync(() => session.Sent.OfType<CharacterList>().Any());
    }

    private static CharacterSummary Summary(ulong id, string name) =>
        new(id, Account, name, Race: 1, Sex: 0, Class: 1, FactionId: 166, Path: 0, Level: 1, WorldId: 0, ZoneId: 0, Position: default,
            Disabled: false, RequiresRename: false, Bones: []);

    private static CharacterCreate Create(string name) => new() { CreationId = 1, Name = name, Path = 1 };
}
