using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingResidencePrivacyLevel)]
public class ClientHousingSetPrivacyLevel : IReadable
{
    public ResidencePrivacyLevel PrivacyLevel { get; private set; }

    public void Read(GamePacketReader reader)
    {
        PrivacyLevel = reader.ReadEnum<ResidencePrivacyLevel>(32u);
    }
}
