using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerEntityDestroy)]
[WireMessage(GameMessageOpcode.ServerEntityDestroy, Direction.ServerToClient)]
public sealed partial class ServerEntityDestroy : IWritable
{
    [UInt(32)] public uint Guid { get; set; }
    [UInt(1)] public bool Unknown0 { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Guid);
        writer.Write(Unknown0);
    }
}
