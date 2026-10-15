using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingTakeMeHome)]
[WireMessage(GameMessageOpcode.ClientHousingTakeMeHome, Direction.ClientToServer)]
public sealed partial class ClientHousingTakeMeHome : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // Zero byte message
    }
}
