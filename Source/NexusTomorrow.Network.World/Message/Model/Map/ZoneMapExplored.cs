using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

/// <summary>
/// Sets which hexes of a zone's map the player has explored.
/// </summary>
[WireMessage(GameMessageOpcode.Server01B4, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ZoneMapExplored
{
    /// <summary>
    /// The zone, as a MapZone id.
    /// </summary>
    [UInt(32)] public uint MapZoneId { get; set; }

    /// <summary>
    /// One bit per hex of the zone's hex rectangle; the client refuses a length other than the rectangle's bit count rounded up to bytes.
    /// </summary>
    [Array(CountBits = 32), UInt(8)] public List<byte> ExploredHexes { get; set; } = [];
}
