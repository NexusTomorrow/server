using System.Collections.Immutable;
using System.Numerics;
using Microsoft.EntityFrameworkCore;

namespace NexusTomorrow.Persistence.Realm;

/// <summary>
/// Loads an account's live characters, ordered by id, with their bones.
/// </summary>
public sealed class LoadCharactersWork : PersistenceWork
{
    private readonly long _accountId;
    private readonly Action<ImmutableArray<CharacterSummary>> _loaded;

    /// <summary>
    /// Create work that hands the characters of <paramref name="accountId"/> to <paramref name="loaded"/> once read.
    /// </summary>
    public LoadCharactersWork(long accountId, Action<ImmutableArray<CharacterSummary>> loaded)
        : base(PersistKey.Account(accountId))
    {
        _accountId = accountId;
        _loaded = loaded;
    }

    /// <summary>
    /// The characters read by the last successful <see cref="ApplyAsync"/>.
    /// </summary>
    internal ImmutableArray<CharacterSummary> Characters { get; set; } = [];

    /// <inheritdoc/>
    public override async Task ApplyAsync(PersistenceContexts contexts, CancellationToken cancellationToken)
    {
        await using RealmContext context = contexts.CreateRealm();
        List<CharacterRow> rows = await context.Characters
            .AsNoTracking()
            .Where(c => c.AccountId == _accountId && c.DeletedAt == null)
            .OrderBy(c => c.Id)
            .Include(c => c.Bones.OrderBy(b => b.BoneIndex))
            .ToListAsync(cancellationToken);
        Characters =
        [
            .. rows.Select(c => new CharacterSummary(
                c.Id, c.AccountId, c.Name, c.Race, c.Sex, c.Class, c.FactionId, c.Path, c.Level, c.WorldId, c.ZoneId,
                new Vector3(c.PositionX, c.PositionY, c.PositionZ), c.Disabled, c.RequiresRename,
                [.. c.Bones.Select(b => b.Value)])),
        ];
    }

    /// <inheritdoc/>
    public override void Complete(WorkResult result)
    {
        if (result.Outcome == WorkOutcome.Committed)
            _loaded(Characters);
    }
}
