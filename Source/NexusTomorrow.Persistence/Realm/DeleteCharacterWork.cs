using Microsoft.EntityFrameworkCore;

namespace NexusTomorrow.Persistence.Realm;

/// <summary>
/// Marks one live character of an account deleted.
/// </summary>
public sealed class DeleteCharacterWork : PersistenceWork
{
    private readonly long _accountId;
    private readonly ulong _characterId;
    private readonly DateTimeOffset _at;
    private readonly Action<WorkResult> _done;

    /// <summary>
    /// Create work that deletes character <paramref name="characterId"/> of <paramref name="accountId"/> as of <paramref name="at"/>
    /// and reports its outcome to <paramref name="done"/>.
    /// </summary>
    public DeleteCharacterWork(long accountId, ulong characterId, DateTimeOffset at, Action<WorkResult> done)
        : base(PersistKey.Account(accountId), PersistKey.Character(characterId))
    {
        _accountId = accountId;
        _characterId = characterId;
        _at = at;
        _done = done;
    }

    /// <inheritdoc/>
    public override async Task ApplyAsync(PersistenceContexts contexts, CancellationToken cancellationToken)
    {
        // Repeatable: a retry after a commit whose acknowledgement was lost matches no live row and changes nothing.
        await using RealmContext context = contexts.CreateRealm();
        await context.Characters
            .Where(c => c.Id == _characterId && c.AccountId == _accountId && c.DeletedAt == null)
            .ExecuteUpdateAsync(set => set.SetProperty(c => c.DeletedAt, _at), cancellationToken);
    }

    /// <inheritdoc/>
    public override void Complete(WorkResult result) => _done(result);
}
