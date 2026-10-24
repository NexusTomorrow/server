using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// A guild member, as the roster shows one.
/// </summary>
[WireStruct]
public sealed partial class GuildMember
{
    /// <summary>
    /// The member, by character id.
    /// </summary>
    public RealmScopedId CharacterId { get; set; } = new();

    /// <summary>
    /// The member's rank, from 0; the roster reports it plus one.
    /// </summary>
    [UInt(32)] public uint Rank { get; set; }

    /// <summary>
    /// The member's name.
    /// </summary>
    [WideString] public string Name { get; set; } = "";

    /// <summary>
    /// The member's sex; for 1 the roster shows the class's female name.
    /// </summary>
    [UInt(2)] public byte Sex { get; set; }

    /// <summary>
    /// The member's class, a Class table id.
    /// </summary>
    [UInt(32)] public uint Class { get; set; }

    /// <summary>
    /// The member's path.
    /// </summary>
    [UInt(32)] public uint Path { get; set; }

    /// <summary>
    /// The member's level.
    /// </summary>
    [UInt(32)] public uint Level { get; set; }

    /// <summary>
    /// When the member was last online, in days from now.
    /// </summary>
    [Float32] public float LastOnline { get; set; }

    /// <summary>
    /// The member's PvP wins.
    /// </summary>
    [UInt(32)] public uint PvpWins { get; set; }

    /// <summary>
    /// The member's PvP losses.
    /// </summary>
    [UInt(32)] public uint PvpLosses { get; set; }

    /// <summary>
    /// The member's PvP draws; GuildTypeLib's GetMyPvpRatings reads them for the player, but the roster shows the losses in their
    /// place.
    /// </summary>
    [UInt(32)] public uint PvpDraws { get; set; }

    /// <summary>
    /// The member's note, which GuildTypeLib's SetMemberNoteSelf sets.
    /// </summary>
    [WideString] public string Note { get; set; } = "";

    [UInt(32)] public uint Unknown12 { get; set; }

    /// <summary>
    /// The community plot the member has reserved, from 0.
    /// </summary>
    [UInt(32)] public uint CommunityReservedPlotIndex { get; set; }
}
