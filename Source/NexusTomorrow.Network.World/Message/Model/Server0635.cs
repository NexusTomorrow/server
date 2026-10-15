using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.Server0635)]
[WireMessage(GameMessageOpcode.Server0635, Direction.ServerToClient)]
public sealed partial class Server0635 : IWritable
{
    [UInt(32)] public uint Unknown0 { get; set; } = 0;

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Unknown0);
    }
}
