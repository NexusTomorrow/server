using System.ComponentModel.DataAnnotations;

namespace NexusTomorrow.Owners.Realm;

/// <summary>
/// The <c>Realm</c> configuration section.
/// </summary>
public sealed class RealmOptions
{
    /// <summary>
    /// The configuration section's name.
    /// </summary>
    public const string Section = "Realm";

    /// <summary>
    /// The realm's id; the client keeps it as its current realm. At most 14 bits.
    /// </summary>
    [Range(0, (1 << 14) - 1)]
    public ushort Id { get; set; } = 1;

    /// <summary>
    /// The realm's name, shown to the player.
    /// </summary>
    public string Name { get; set; } = "NexusTomorrow";

    /// <summary>
    /// The realm's flags; bit 4 keeps characters to one faction.
    /// </summary>
    public uint Flags { get; set; }

    /// <summary>
    /// 0 PvE, 1 PvP.
    /// </summary>
    [Range(0, 1)]
    public byte PvpType { get; set; }

    /// <summary>
    /// A note about the realm, as a text id; 0 for none.
    /// </summary>
    public uint NoteTextId { get; set; }

    /// <summary>
    /// How many live characters an account may hold.
    /// </summary>
    [Range(1, 50)]
    public int MaxCharactersPerAccount { get; set; } = 12;

    /// <summary>
    /// Where a new character stands, by faction. A faction with no entry starts in world 0 at the origin.
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
