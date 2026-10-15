using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicComplete)]
[WireMessage(GameMessageOpcode.ServerCinematicComplete, Direction.ServerToClient)]
public sealed partial class ServerCinematicComplete : IWritable
{
    public void Write(GamePacketWriter writer)
    {
        // zero byte message
    }
}
