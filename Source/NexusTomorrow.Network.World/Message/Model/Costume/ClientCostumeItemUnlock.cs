using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[WireMessage(GameMessageOpcode.ClientCostumeItemUnlock, Direction.ClientToServer)]
public sealed partial class ClientCostumeItemUnlock
{
    public ItemLocation Location { get; } = new();
}
