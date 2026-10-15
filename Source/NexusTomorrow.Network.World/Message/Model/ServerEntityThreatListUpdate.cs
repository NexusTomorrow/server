using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// Values from this packet are stored in a "global" threat list which is only used for the current target.
/// This is different from <see cref="ServerEntityThreatUpdate"/> where values are stored against the entity object in the client.
/// </summary>
[WireMessage(GameMessageOpcode.ServerEntityThreatListUpdate, Direction.ServerToClient)]
public sealed partial class ServerEntityThreatListUpdate
{
    [UInt(32)] public uint SrcUnitId { get; set; }
    [FixedArray(5), UInt(32)] public uint[] ThreatUnitIds { get; set; } = new uint[5];
    [FixedArray(5), UInt(32)] public uint[] ThreatLevels { get; set; } = new uint[5];
}
