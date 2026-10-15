using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

// Sent either the player's identity or the player's name, never both
[Message(GameMessageOpcode.ClientHousingNeighbourEvict)]
public class ClientHousingNeighbourEvict : IReadable
{
    public Identity PlayerIdentity { get; private set; } = new();
    public string PlayerName { get; private set; }

    public void Read(GamePacketReader reader)
    {
        PlayerIdentity.Read(reader);
        PlayerName = reader.ReadWideString();
    }
}
