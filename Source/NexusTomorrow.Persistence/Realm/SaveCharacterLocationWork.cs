using Microsoft.EntityFrameworkCore;

namespace NexusTomorrow.Persistence.Realm;

/// <summary>
/// Writes where a live character stands; a newer location for the same character, queued while this one waits, replaces this
/// one's and adds its completion, so the newest location is written once and every caller hears of it.
/// </summary>
public sealed class SaveCharacterLocationWork : PersistenceWork
{
    private readonly ulong _characterId;
    private readonly List<Action<WorkResult>> _done;

    /// <summary>
    /// Create work that saves <paramref name="location"/> for character <paramref name="characterId"/> and reports its outcome to
    /// <paramref name="done"/>.
    /// </summary>
    public SaveCharacterLocationWork(ulong characterId, CharacterLocation location, Action<WorkResult> done)
        : base(PersistKey.Character(characterId))
    {
        _characterId = characterId;
        Location = location;
        _done = [done];
    }

    /// <summary>
    /// The location this work writes.
    /// </summary>
    internal CharacterLocation Location { get; private set; }

    /// <inheritdoc/>
    public override async Task ApplyAsync(PersistenceContexts contexts, CancellationToken cancellationToken)
    {
        // Every attempt writes the same values, so a retry is harmless.
        CharacterLocation location = Location;
        await using RealmContext context = contexts.CreateRealm();
        await context.Characters
            .Where(c => c.Id == _characterId && c.DeletedAt == null)
            .ExecuteUpdateAsync(
                set => set
                    .SetProperty(c => c.WorldId, location.WorldId)
                    .SetProperty(c => c.ZoneId, location.ZoneId)
                    .SetProperty(c => c.PositionX, location.Position.X)
                    .SetProperty(c => c.PositionY, location.Position.Y)
                    .SetProperty(c => c.PositionZ, location.Position.Z)
                    .SetProperty(c => c.Facing, location.Facing),
                cancellationToken);
    }

    /// <inheritdoc/>
    public override bool TryMerge(PersistenceWork newer)
    {
        if (newer is not SaveCharacterLocationWork save || save._characterId != _characterId)
            return false;

        Location = save.Location;
        _done.AddRange(save._done);
        return true;
    }

    /// <inheritdoc/>
    public override void Complete(WorkResult result)
    {
        foreach (Action<WorkResult> done in _done)
            done(result);
    }
}
