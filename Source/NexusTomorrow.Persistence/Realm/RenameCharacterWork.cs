using Microsoft.EntityFrameworkCore;

namespace NexusTomorrow.Persistence.Realm;

/// <summary>
/// Renames one live, marked character of an account and clears its rename mark; an unmarked character is left as it is. A taken name ends as <see cref="WorkOutcome.Refused"/> by
/// <see cref="RealmContext.LiveNameIndex"/>.
/// </summary>
public sealed class RenameCharacterWork : PersistenceWork
{
    private readonly long _accountId;
    private readonly ulong _characterId;
    private readonly string _name;
    private readonly Action<WorkResult> _done;

    /// <summary>
    /// Create work that renames character <paramref name="characterId"/> of <paramref name="accountId"/> to <paramref name="name"/>
    /// and reports its outcome to <paramref name="done"/>.
    /// </summary>
    public RenameCharacterWork(long accountId, ulong characterId, string name, Action<WorkResult> done)
        : base(PersistKey.Account(accountId), PersistKey.Character(characterId))
    {
        _accountId = accountId;
        _characterId = characterId;
        _name = name;
        _done = done;
    }

    /// <inheritdoc/>
    public override async Task ApplyAsync(PersistenceContexts contexts, CancellationToken cancellationToken)
    {
        // Only a marked character renames, so a repeat of a rename that already landed changes no row.
        await using RealmContext context = contexts.CreateRealm();
        await context.Characters
            .Where(c => c.Id == _characterId && c.AccountId == _accountId && c.DeletedAt == null && c.RequiresRename)
            .ExecuteUpdateAsync(set => set.SetProperty(c => c.Name, _name).SetProperty(c => c.RequiresRename, false), cancellationToken);
    }

    /// <inheritdoc/>
    public override void Complete(WorkResult result) => _done(result);
}
