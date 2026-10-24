using System.Numerics;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Owners.Text;
using NexusTomorrow.Owners.World;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Realm;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Owners.Tests;

public sealed class RealmChatTests : IAsyncLifetime
{
    private const long Account = 7;

    // The Say channel's type.
    private const ushort Say = 4;

    private readonly FakeWriteBehind _db = new();
    private readonly FakeClientSession _session = new();
    private readonly FakeMapDirectory _maps = new();
    private readonly RecordingLogger<RealmOwner> _log = new();
    private readonly RealmOwner _owner;

    public RealmChatTests()
    {
        // A made-up word, never one of the client's.
        var filter = new WordFilter([(0, "blorp")], NullLogger<WordFilter>.Instance);
        _owner = new RealmOwner(
            _db, new FixedCreationCatalog([]), filter, FixedEntitlementCaps.Defaults(), _maps, new FakeTimeProvider(), Options.Create(new RealmOptions()), _log);
    }

    public ValueTask InitializeAsync()
    {
        _owner.Start();
        return ValueTask.CompletedTask;
    }

    public async ValueTask DisposeAsync() => await _owner.StopAsync().WaitAsync(TimeSpan.FromSeconds(10));

    [Fact]
    public async Task ASayLineGoesToTheSpeakersMap()
    {
        FakeMapInbox map = await EnterAsync();

        _owner.Post(new ChatSaid(Account, _session, Say, "hello", 3));

        await Eventually.TrueAsync(() => map.Posted.Count == 2);
        LocalChat chat = Assert.IsType<LocalChat>(map.Posted[1]);
        Assert.Same(_session, chat.Session);
        Assert.Equal(5UL, chat.CharacterId);
        Assert.Equal((ushort)3, chat.ChatId);
        Assert.Equal("hello", chat.Text);
    }

    [Fact]
    public async Task ALineOnAnotherChannelIsDropped()
    {
        FakeMapInbox map = await EnterAsync();

        _owner.Post(new ChatSaid(Account, _session, 5, "blue fox", 3));

        await Eventually.TrueAsync(() => _owner.DroppedChatLines == 1);
        Assert.DoesNotContain(map.Posted, posted => posted is LocalChat);
        Assert.DoesNotContain(_log.Entries, entry => entry.Contains("blue fox", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task AnEmptyOrOverlongLineIsDropped()
    {
        FakeMapInbox map = await EnterAsync();

        _owner.Post(new ChatSaid(Account, _session, Say, "", 3));
        _owner.Post(new ChatSaid(Account, _session, Say, new string('a', 500), 4));

        await Eventually.TrueAsync(() => _owner.DroppedChatLines == 2);
        Assert.DoesNotContain(map.Posted, posted => posted is LocalChat);
    }

    [Fact]
    public async Task AFilteredLineIsDropped()
    {
        FakeMapInbox map = await EnterAsync();

        _owner.Post(new ChatSaid(Account, _session, Say, "say blorp", 3));

        await Eventually.TrueAsync(() => _owner.DroppedChatLines == 1);
        Assert.DoesNotContain(map.Posted, posted => posted is LocalChat);
        Assert.DoesNotContain(_log.Entries, entry => entry.Contains("blorp", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task ASpeakerWithNoCharacterInAMapIsDropped()
    {
        FakeMapInbox map = _maps.Add(1);
        await ListAsync();

        _owner.Post(new ChatSaid(Account, _session, Say, "hello", 3));

        await Eventually.TrueAsync(() => _owner.DroppedChatLines == 1);
        Assert.Empty(map.Posted);
    }

    // Lists character 5 in world 1, selects it, and lands its load, so the realm posts its arrival to world 1's map.
    private async Task<FakeMapInbox> EnterAsync()
    {
        FakeMapInbox map = _maps.Add(1);
        await ListAsync();
        _owner.Post(new SelectCharacter(Account, _session, 5));
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharacterWork>().Any());
        LoadCharacterWork load = _db.Take<LoadCharacterWork>();
        load.Character = new CharacterRecord(
            5, Account, "Rebaser Head", Race: 1, Sex: 0, Class: 1, FactionId: 166, Path: 0, Level: 1, TotalXp: 0,
            new CharacterLocation(1, 0, new Vector3(1, 2, 3), 0));
        load.Complete(WorkResult.Committed);
        await Eventually.TrueAsync(() => map.Posted.Count == 1);
        Assert.IsType<CharacterArrived>(map.Posted[0]);
        return map;
    }

    private async Task ListAsync()
    {
        _owner.Post(new ListCharacters(Account, _session));
        await Eventually.TrueAsync(() => _db.Queued.OfType<LoadCharactersWork>().Any());
        LoadCharactersWork load = _db.Take<LoadCharactersWork>();
        load.Characters =
        [
            new CharacterSummary(5, Account, "Rebaser Head", Race: 1, Sex: 0, Class: 1, FactionId: 166, Path: 0, Level: 1, WorldId: 1,
                ZoneId: 0, Position: default, Disabled: false, RequiresRename: false, Bones: []),
        ];
        load.Complete(WorkResult.Committed);
        await Eventually.TrueAsync(() => _session.Sent.OfType<CharacterList>().Any());
    }
}
