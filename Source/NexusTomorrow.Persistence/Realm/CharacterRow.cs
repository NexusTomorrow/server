namespace NexusTomorrow.Persistence.Realm;

#pragma warning disable CS1591 // Data-shape code: the type's summary names the table; a column's name is its meaning (code standard, Documentation).

/// <summary>
/// <c>realm.character</c>: a character at rest; <see cref="DeletedAt"/> set means deleted.
/// </summary>
public sealed class CharacterRow
{
    public ulong Id { get; set; }

    public long AccountId { get; set; }

    public string Name { get; set; } = "";

    public uint CreationId { get; set; }

    public byte Race { get; set; }

    public byte Sex { get; set; }

    public byte Class { get; set; }

    public uint FactionId { get; set; }

    public byte Path { get; set; }

    public uint Level { get; set; }

    public uint TotalXp { get; set; }

    public ushort WorldId { get; set; }

    public ushort ZoneId { get; set; }

    public float PositionX { get; set; }

    public float PositionY { get; set; }

    public float PositionZ { get; set; }

    public bool Disabled { get; set; }

    public bool RequiresRename { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public List<CharacterCustomizationRow> Customizations { get; set; } = [];

    public List<CharacterBoneRow> Bones { get; set; } = [];
}
