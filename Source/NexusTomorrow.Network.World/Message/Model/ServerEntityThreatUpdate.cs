using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// Values from this packet are stored against the entity object in the client.
/// This is different from <see cref="ServerEntityThreatListUpdate"/> where values are stored in a "global" threat list which is only used for the current target.
/// </summary>
[Message(GameMessageOpcode.ServerEntityThreatUpdate)]
public class ServerEntityThreatUpdate : IWritable
{
    public uint UnitId { get; set; }
    public uint TargetId { get; set; }
    public uint ThreatLevel { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(TargetId);
        writer.Write(ThreatLevel);
    }
}
