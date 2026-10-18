namespace NexusTomorrow.Persistence.Realm;

#pragma warning disable CS1591 // Data-shape code: the type's summary names the table; a column's name is its meaning (code standard, Documentation).

/// <summary>
/// <c>realm.character_bone</c>: one bone slider value, in creation order.
/// </summary>
public sealed class CharacterBoneRow
{
    public ulong CharacterId { get; set; }

    public int BoneIndex { get; set; }

    public float Value { get; set; }
}
