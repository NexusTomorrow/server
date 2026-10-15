using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[Message(GameMessageOpcode.ClientCharacterList)]
[WireMessage(GameMessageOpcode.ClientCharacterList, Direction.ClientToServer)]
public sealed partial class ClientCharacterList : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // Zero byte message
    }
}
