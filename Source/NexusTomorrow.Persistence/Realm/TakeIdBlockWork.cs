using Microsoft.EntityFrameworkCore;

namespace NexusTomorrow.Persistence.Realm;

/// <summary>
/// Takes the next block of <see cref="RealmContext.CharacterIdBlock"/> character ids off the realm's id sequence.
/// </summary>
public sealed class TakeIdBlockWork : PersistenceWork
{
    // The sequence steps by RealmContext.CharacterIdBlock, so one nextval is the first id of a block.
    private const string NextBlockSql = "SELECT nextval('realm.character_id_seq') AS \"Value\"";

    private readonly Action<IdBlock> _taken;

    /// <summary>
    /// Create work that hands the block to <paramref name="taken"/> once committed.
    /// </summary>
    public TakeIdBlockWork(Action<IdBlock> taken)
        : base(PersistKey.Named("character-ids"))
    {
        _taken = taken;
    }

    /// <summary>
    /// The block taken by the last successful <see cref="ApplyAsync"/>.
    /// </summary>
    internal IdBlock Block { get; set; }

    /// <inheritdoc/>
    public override async Task ApplyAsync(PersistenceContexts contexts, CancellationToken cancellationToken)
    {
        await using RealmContext context = contexts.CreateRealm();
        long value = await context.Database.SqlQueryRaw<long>(NextBlockSql).SingleAsync(cancellationToken);
        Block = new IdBlock((ulong)value, RealmContext.CharacterIdBlock);
    }

    /// <inheritdoc/>
    public override void Complete(WorkResult result)
    {
        if (result.Outcome == WorkOutcome.Committed)
            _taken(Block);
    }
}
