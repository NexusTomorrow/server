using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingDoorInteraction)]
public class ClientHousingDoorInteraction : IReadable
{
    public Identity ResidenceIdentity { get; } = new();

    public void Read(GamePacketReader reader)
    {
        ResidenceIdentity.Read(reader);
    }
}
