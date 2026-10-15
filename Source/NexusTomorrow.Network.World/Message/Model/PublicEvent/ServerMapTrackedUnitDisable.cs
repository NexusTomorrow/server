using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ServerMapTrackedUnitDisable)]
[WireMessage(GameMessageOpcode.ServerMapTrackedUnitDisable, Direction.ServerToClient)]
public sealed partial class ServerMapTrackedUnitDisable : IWritable
{
    [UInt(32)] public uint TrackedUnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TrackedUnitId);
    }
}
