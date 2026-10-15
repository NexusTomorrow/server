using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingManager0x5B0)]
[WireMessage(GameMessageOpcode.ServerMatchingManager0x5B0, Direction.ServerToClient)]
public sealed partial class ServerMatchingManager0x5B0 : IWritable
{
    [UInt(1)] public bool Unknown { get; set; } // possibly indicates InMatchingGameGroup

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Unknown);
    }
}
