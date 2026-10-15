using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientMatchingMatchReadyResponse)]
public class ClientMatchingGameReadyResponse : IReadable
{
    public bool Response { get; private set; } // decline = false, join = true

    public void Read(GamePacketReader reader)
    {
        Response = reader.ReadBit();
    }
}
