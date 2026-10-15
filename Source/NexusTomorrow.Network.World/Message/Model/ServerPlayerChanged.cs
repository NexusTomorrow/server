using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerPlayerChanged)]
[WireMessage(GameMessageOpcode.ServerPlayerChanged, Direction.ServerToClient)]
public sealed partial class ServerPlayerChanged : IWritable
{
    [UInt(32)] public uint Guid { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Guid);
        writer.Write(Unknown1);
    }
}
