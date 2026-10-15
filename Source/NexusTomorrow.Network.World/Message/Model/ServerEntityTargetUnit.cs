using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// Values from this packet are handled in the same client function as <see cref="ServerEntityThreatUpdate"/>.
/// Additionally it will set the target id on the entity object in the client.
/// </summary>
[Message(GameMessageOpcode.ServerEntityTargetUnit)]
[WireMessage(GameMessageOpcode.ServerEntityTargetUnit, Direction.ServerToClient)]
public sealed partial class ServerEntityTargetUnit : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(32)] public uint NewTargetId { get; set; }
    [UInt(32)] public uint ThreatLevel { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(NewTargetId);
        writer.Write(ThreatLevel);
    }
}
