using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

// Sent either the player's identity or the player's name, never both
[WireMessage(GameMessageOpcode.ClientHousingNeighbourEvict, Direction.ClientToServer)]
public sealed partial class ClientHousingNeighbourEvict
{
    public Identity PlayerIdentity { get; private set; } = new();
    [WideString] public string PlayerName { get; private set; }
}
