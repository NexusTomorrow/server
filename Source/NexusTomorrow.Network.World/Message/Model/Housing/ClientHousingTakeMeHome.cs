using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingTakeMeHome)]
public class ClientHousingTakeMeHome : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // Zero byte message
    }
}
