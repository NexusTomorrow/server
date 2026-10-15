using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingRequestCommunityPlacement)]
public class ClientHousingRequestCommunityPlacement : IReadable
{
    public Identity ResidenceIdentity { get; } = new();
    public uint PropertyIndex { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ResidenceIdentity.Read(reader);
        PropertyIndex = reader.ReadUInt();
    }
}
