using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Seems be to used after a cinematic
[Message(GameMessageOpcode.ServerLoadingScreen)]
[WireMessage(GameMessageOpcode.ServerLoadingScreen, Direction.ServerToClient)]
public sealed partial class ServerLoadingScreen : IWritable
{
    public void Write(GamePacketWriter writer)
    {
        // zero byte message
    }
}
