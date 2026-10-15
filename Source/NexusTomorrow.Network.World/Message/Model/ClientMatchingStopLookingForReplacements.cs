using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientMatchingStopLookingForReplacements)]
[WireMessage(GameMessageOpcode.ClientMatchingStopLookingForReplacements, Direction.ClientToServer)]
public sealed partial class ClientMatchingStopLookingForReplacements : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // Zero byte message
    }
}
