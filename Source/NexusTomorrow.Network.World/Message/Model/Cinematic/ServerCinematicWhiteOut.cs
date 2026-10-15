using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicWhiteOut)]
[WireMessage(GameMessageOpcode.ServerCinematicWhiteOut, Direction.ServerToClient)]
public sealed partial class ServerCinematicWhiteOut : IWritable
{
    public void Write(GamePacketWriter writer)
    {
        // zero length message
    }
}
