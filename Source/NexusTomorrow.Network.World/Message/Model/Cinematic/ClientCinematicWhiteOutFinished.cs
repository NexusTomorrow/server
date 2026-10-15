using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ClientCinematicWhiteOutFinished)]
[WireMessage(GameMessageOpcode.ClientCinematicWhiteOutFinished, Direction.ClientToServer)]
public sealed partial class ClientCinematicWhiteOutFinished : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // zero byte message
    }
}
