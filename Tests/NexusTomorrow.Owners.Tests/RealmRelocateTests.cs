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

public sealed class RealmRelocateTests : IAsyncLifetime
{
    private const long Account = 7;
    private static readonly TimeSpan s_bound = TimeSpan.FromSeconds(10);

    private readonly FakeWriteBehind _db = new();
    private readonly FakeClientSession _session = new();
    private readonly FakeMapDirectory _maps = new();
    private readonly RealmOwner _owner;

    public RealmRelocateTests()
    {
        var filter = new WordFilter([], NullLogger<WordFilter>.Instance);
        _owner = new RealmOwner(
            _db, new FixedCreationCatalog([]), filter, FixedEntitlementCaps.Defaults(), _maps, new FakeTimeProvider(),
            Options.Create(new RealmOptions()), NullLogger<RealmOwner>.Instance);
    }

    public ValueTask InitializeAsync()
    {
        _owner.Start();
        return ValueTask.CompletedTask;
    }

    public async ValueTask DisposeAsync() => await _owner.StopAsync().WaitAsync(s_bound);

    [Fact]
    public async Task ARelocateGoesToTheMapHoldingTheCharacter()
    {
        FakeMapInbox map = await EnterAsync();
        var reply = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

        _owner.Post(new RelocateRequested(5, new Vector3(5, 6, 7), 1, reply));

        await Eventually.TrueAsync(() => map.Posted.Count == 2);
        RelocateCharacter relocate = Assert.IsType<RelocateCharacter>(map.Posted[1]);
        Assert.Equal(5UL, relocate.CharacterId);
        Assert.Equal(new Vector3(5, 6, 7), relocate.Position);
        Assert.Equal(1f, relocate.Facing);
        Assert.Same(reply, relocate.Reply);
        Assert.False(reply.Task.IsCompleted);
    }

    [Fact]
    public async Task ARelocateOfACharacterAtRestAnswersFalse()
    {
        FakeMapInbox map = _maps.Add(1);
        await ListAsync();
        var reply = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

        _owner.Post(new RelocateRequested(5, new Vector3(5, 6, 7), 1, reply));

        Assert.False(await reply.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken));
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
