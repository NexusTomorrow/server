using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ClientHousingResidencePrivacyLevel, Direction.ClientToServer)]
public sealed partial class ClientHousingSetPrivacyLevel
{
    [UInt(32)] public ResidencePrivacyLevel PrivacyLevel { get; private set; }
}
