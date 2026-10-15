using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ClientHousingDoorInteraction, Direction.ClientToServer)]
public sealed partial class ClientHousingDoorInteraction
{
    public Identity ResidenceIdentity { get; } = new();
}
