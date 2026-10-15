using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[Message(GameMessageOpcode.ServerGenericError)]
[WireMessage(GameMessageOpcode.ServerGenericError, Direction.ServerToClient)]
public sealed partial class ServerGenericError : IWritable
{
    [UInt(8)] public GenericError Error { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Error, 8u);
    }
}
