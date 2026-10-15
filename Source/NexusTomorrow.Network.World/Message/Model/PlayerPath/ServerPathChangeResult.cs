using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;


[Message(GameMessageOpcode.ServerPathChangeResult)]
[WireMessage(GameMessageOpcode.ServerPathChangeResult, Direction.ServerToClient)]
public sealed partial class ServerPathChangeResult : IWritable
{
    [UInt(8)] public GenericError Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Result, 8);
    }
}
