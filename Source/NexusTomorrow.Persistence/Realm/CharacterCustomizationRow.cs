namespace NexusTomorrow.Persistence.Realm;

#pragma warning disable CS1591 // Data-shape code: the type's summary names the table; a column's name is its meaning (code standard, Documentation).

/// <summary>
/// <c>realm.character_customization</c>: one look choice made at creation.
/// </summary>
public sealed class CharacterCustomizationRow
{
    public ulong CharacterId { get; set; }

    public uint Label { get; set; }

    public uint Value { get; set; }
}
