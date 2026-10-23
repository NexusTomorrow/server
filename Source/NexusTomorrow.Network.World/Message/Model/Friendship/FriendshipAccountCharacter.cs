using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// One character on an account friend's account.
/// </summary>
[WireStruct]
public sealed partial class FriendshipAccountCharacter
{
    /// <summary>
    /// The character's name.
    /// </summary>
    [WideString] public string Name { get; set; } = "";

    /// <summary>
    /// The character, by realm and character id. <see cref="FriendshipAccountCharacterRemoved"/>,
    /// <see cref="FriendshipAccountCharacterLevel"/> and <see cref="FriendshipAccountCharacterZones"/> name the character by it.
    /// </summary>
    public RealmScopedId CharacterId { get; set; } = new();

    /// <summary>
    /// The character's class id, which add-ons read as nClassId.
    /// </summary>
    [UInt(14)] public ushort ClassId { get; set; }

    /// <summary>
    /// The character's race id, which add-ons read as nRaceId.
    /// </summary>
    [UInt(14)] public ushort RaceId { get; set; }

    /// <summary>
    /// The character's path id, which add-ons read as nPathId.
    /// </summary>
    [UInt(32)] public uint PathId { get; set; }

    /// <summary>
    /// The character's level, which add-ons read as nLevel.
    /// </summary>
    [UInt(32)] public uint Level { get; set; }

    /// <summary>
    /// The zone the character is in, a WorldZone table id, which add-ons see as strWorldZone.
    /// </summary>
    [UInt(15)] public ushort WorldZoneId { get; set; }

    /// <summary>
    /// The character's faction id, which add-ons read as nFactionId.
    /// </summary>
    [UInt(14)] public ushort FactionId { get; set; }
}
