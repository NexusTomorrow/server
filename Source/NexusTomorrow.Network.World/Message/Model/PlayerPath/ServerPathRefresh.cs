using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;


[Message(GameMessageOpcode.ServerPathRefresh)]
[WireMessage(GameMessageOpcode.ServerPathRefresh, Direction.ServerToClient)]
public sealed partial class ServerPathRefresh : IWritable
{
    public void Write(GamePacketWriter writer)
    {
        // zero byte message
    }
}
