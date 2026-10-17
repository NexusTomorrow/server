namespace NexusTomorrow.Persistence.Realm;

/// <summary>
/// Writes a new character with its customizations and bones in one <c>SaveChanges</c>. A taken name ends as
/// <see cref="WorkOutcome.Refused"/> by <see cref="RealmContext.LiveNameIndex"/>.
/// </summary>
public sealed class CreateCharacterWork : PersistenceWork
{
    private readonly NewCharacter _character;
    private readonly Action<WorkResult> _done;

    /// <summary>
    /// Create work that writes <paramref name="character"/> and reports its outcome to <paramref name="done"/>.
    /// </summary>
    public CreateCharacterWork(NewCharacter character, Action<WorkResult> done)
        : base(PersistKey.Account(character.AccountId), PersistKey.Character(character.Id))
    {
        _character = character;
        _done = done;
    }

    /// <inheritdoc/>
    public override async Task ApplyAsync(PersistenceContexts contexts, CancellationToken cancellationToken)
    {
        // A fresh context each attempt, so a retry never carries a failed attempt's tracked rows.
        await using RealmContext context = contexts.CreateRealm();
        var row = new CharacterRow
        {
            Id = _character.Id,
            AccountId = _character.AccountId,
            Name = _character.Name,
            CreationId = _character.CreationId,
            Race = _character.Race,
            Sex = _character.Sex,
            Class = _character.Class,
            FactionId = _character.FactionId,
            Path = _character.Path,
            Level = _character.Level,
            TotalXp = _character.TotalXp,
            WorldId = _character.WorldId,
            ZoneId = _character.ZoneId,
            PositionX = _character.Position.X,
            PositionY = _character.Position.Y,
            PositionZ = _character.Position.Z,
            CreatedAt = _character.CreatedAt,
        };
        foreach ((uint label, uint value) in _character.Customizations)
            row.Customizations.Add(new CharacterCustomizationRow { CharacterId = _character.Id, Label = label, Value = value });
        for (int i = 0; i < _character.Bones.Length; i++)
            row.Bones.Add(new CharacterBoneRow { CharacterId = _character.Id, BoneIndex = i, Value = _character.Bones[i] });

        context.Characters.Add(row);
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public override void Complete(WorkResult result) => _done(result);
}
