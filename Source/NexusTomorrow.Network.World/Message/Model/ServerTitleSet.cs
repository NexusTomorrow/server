using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerTitleSet)]
[WireMessage(GameMessageOpcode.ServerTitleSet, Direction.ServerToClient)]
public sealed partial class ServerTitleSet : IWritable
{
    [UInt(32)] public uint Guid { get; set; }
    [UInt(14)] public ushort Title { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Guid);
        writer.Write(Title, 14);
    }
}
