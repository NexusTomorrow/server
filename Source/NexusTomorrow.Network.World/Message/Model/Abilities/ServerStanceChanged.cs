using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[Message(GameMessageOpcode.ServerStanceChanged)]
[WireMessage(GameMessageOpcode.ServerStanceChanged, Direction.ServerToClient)]
public sealed partial class ServerStanceChanged : IWritable
{
    [UInt(2)] public byte InnateIndex { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(InnateIndex, 2u);
    }
}
