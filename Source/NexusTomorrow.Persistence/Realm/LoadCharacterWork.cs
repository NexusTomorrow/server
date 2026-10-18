using System.Numerics;
using Microsoft.EntityFrameworkCore;

namespace NexusTomorrow.Persistence.Realm;

/// <summary>
/// Loads one live character of an account in full, under the character's own key, so it runs after any save of that character
/// queued before it; a deleted character or another account's loads as null.
/// </summary>
public sealed class LoadCharacterWork : PersistenceWork
{
    private readonly ulong _characterId;
    private readonly long _accountId;
    private readonly Action<CharacterRecord?> _loaded;

    /// <summary>
    /// Create work that hands character <paramref name="characterId"/> of <paramref name="accountId"/> to <paramref name="loaded"/>
    /// once read, or null when there is no such live character.
    /// </summary>
    public LoadCharacterWork(ulong characterId, long accountId, Action<CharacterRecord?> loaded)
        : base(PersistKey.Character(characterId))
    {
        _characterId = characterId;
        _accountId = accountId;
        _loaded = loaded;
    }

    /// <summary>
    /// The character read by the last successful <see cref="ApplyAsync"/>, or null.
    /// </summary>
    internal CharacterRecord? Character { get; set; }

    /// <inheritdoc/>
    public override async Task ApplyAsync(PersistenceContexts contexts, CancellationToken cancellationToken)
    {
        await using RealmContext context = contexts.CreateRealm();
        CharacterRow? row = await context.Characters
            .AsNoTracking()
            .Where(c => c.Id == _characterId && c.AccountId == _accountId && c.DeletedAt == null)
            .SingleOrDefaultAsync(cancellationToken);
        Character = row is null
            ? null
            : new CharacterRecord(
                row.Id, row.AccountId, row.Name, row.Race, row.Sex, row.Class, row.FactionId, row.Path, row.Level, row.TotalXp,
                new CharacterLocation(row.WorldId, row.ZoneId, new Vector3(row.PositionX, row.PositionY, row.PositionZ), row.Facing));
    }

    /// <inheritdoc/>
    public override void Complete(WorkResult result)
    {
        if (result.Outcome == WorkOutcome.Committed)
            _loaded(Character);
    }
}
