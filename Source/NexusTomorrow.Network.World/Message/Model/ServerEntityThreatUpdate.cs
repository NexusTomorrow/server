using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// Values from this packet are stored against the entity object in the client.
/// This is different from <see cref="ServerEntityThreatListUpdate"/> where values are stored in a "global" threat list which is only used for the current target.
/// </summary>
[WireMessage(GameMessageOpcode.ServerEntityThreatUpdate, Direction.ServerToClient)]
public sealed partial class ServerEntityThreatUpdate
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(32)] public uint TargetId { get; set; }
    [UInt(32)] public uint ThreatLevel { get; set; }
}
