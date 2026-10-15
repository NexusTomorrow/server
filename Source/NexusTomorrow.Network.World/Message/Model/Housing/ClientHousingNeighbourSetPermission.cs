using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

// Sent either the player's identity or the player's name, never both
[Message(GameMessageOpcode.ClientHousingNeighbourSetPermission)]
public class ClientHousingNeighbourSetPermission : IReadable
{
    public Identity Identity { get; private set; } = new();
    public string Name { get; private set; }
    public NeighbourPermissionLevel Permissions { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Identity.Read(reader);
        Name = reader.ReadWideString();
        Permissions = reader.ReadEnum<NeighbourPermissionLevel>(32u);
    }
}
