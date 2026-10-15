using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingResidencePrivacyLevel)]
[WireMessage(GameMessageOpcode.ClientHousingResidencePrivacyLevel, Direction.ClientToServer)]
public sealed partial class ClientHousingSetPrivacyLevel : IReadable
{
    [UInt(32)] public ResidencePrivacyLevel PrivacyLevel { get; private set; }

    public void Read(GamePacketReader reader)
    {
        PrivacyLevel = reader.ReadEnum<ResidencePrivacyLevel>(32u);
    }
}
