using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ClientHousingVendorList, Direction.ClientToServer)]
public sealed partial class ClientHousingVendorList
{
    public Identity ResidenceIdentity { get; private set; } = new();
    [UInt(2)] public byte ResidenceIsBroker { get; private set; } // 0 = normal residences, 1 is when is ResidenceType.Broker
}
