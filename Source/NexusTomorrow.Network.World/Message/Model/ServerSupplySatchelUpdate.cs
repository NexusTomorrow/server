using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;


[Message(GameMessageOpcode.ServerSupplySatchelUpdate)]
[WireMessage(GameMessageOpcode.ServerSupplySatchelUpdate, Direction.ServerToClient)]
public sealed partial class ServerSupplySatchelUpdate : IWritable
{
    [UInt(14)] public ushort MaterialId { get; set; }
    [UInt(16)] public ushort StackCount { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(MaterialId, 14u);
        writer.Write(StackCount);
    }
}
