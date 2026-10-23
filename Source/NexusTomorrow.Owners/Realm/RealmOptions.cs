using NexusTomorrow.Settings;

namespace NexusTomorrow.Owners.Realm;

/// <summary>
/// The <c>Realm</c> configuration section.
/// </summary>
[SettingsSection(Section, 3, "Owners", "Owners#knobs")]
public sealed class RealmOptions
{
    /// <summary>
    /// The configuration section's name.
    /// </summary>
    public const string Section = "Realm";

    /// <summary>
    /// The realm's id, which the client keeps as its current realm and a character's unit carries.
    /// </summary>
    /// <remarks>
    /// 1 to 16383: the client refuses 0, and the id is 14 bits wide.
    /// </remarks>
    [Between(1, (1 << 14) - 1)]
    public ushort Id { get; set; } = 1;

    /// <summary>
    /// The realm's name, shown to the player.
    /// </summary>
    [RequiredSetting]
    public string Name { get; set; } = "NexusTomorrow";

    /// <summary>
    /// The realm's flags; bit 4 keeps characters to one faction.
    /// </summary>
    public uint Flags { get; set; }

    /// <summary>
    /// 0 PvE, 1 PvP.
    /// </summary>
    [Between(0, 1)]
    public byte PvpType { get; set; }

    /// <summary>
    /// A note about the realm, as a text id; 0 for none.
    /// </summary>
    public uint NoteTextId { get; set; }

    /// <summary>
    /// How many live characters an account may hold.
    /// </summary>
    [Between(1, 50)]
    public int MaxCharactersPerAccount { get; set; } = 12;

    /// <summary>
    /// Where a new character stands, by faction; a faction with no entry starts in world 0 at the origin.
    /// </summary>
    public List<StartLocation> StartLocations { get; set; } = [];
}

/// <summary>
/// Where new characters of one faction start.
/// </summary>
public sealed class StartLocation
{
    /// <summary>
    /// The faction this location serves.
    /// </summary>
    public uint FactionId { get; set; }

    /// <summary>
    /// The world id.
    /// </summary>
    public ushort WorldId { get; set; }

    /// <summary>
    /// The zone id.
    /// </summary>
    public ushort ZoneId { get; set; }

    /// <summary>
    /// The x coordinate.
    /// </summary>
    public float X { get; set; }

    /// <summary>
    /// The y coordinate.
    /// </summary>
    public float Y { get; set; }

    /// <summary>
    /// The z coordinate.
    /// </summary>
    public float Z { get; set; }
}
