using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// A guild's PvP record, as GuildTypeLib's GetPvpRatings reports it.
/// </summary>
[WireStruct]
public sealed partial class GuildPvpRecord
{
    /// <summary>
    /// The guild's wins.
    /// </summary>
    [UInt(32)] public uint Wins { get; set; }

    /// <summary>
    /// The guild's losses.
    /// </summary>
    [UInt(32)] public uint Losses { get; set; }

    /// <summary>
    /// The guild's draws.
    /// </summary>
    [UInt(32)] public uint Draws { get; set; }

    /// <summary>
    /// The guild's rating.
    /// </summary>
    [UInt(32)] public uint Rating { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown4 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown5 { get; set; }
}
