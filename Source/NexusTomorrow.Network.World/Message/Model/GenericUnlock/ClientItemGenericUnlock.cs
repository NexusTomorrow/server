using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock;

[WireMessage(GameMessageOpcode.ClientItemGenericUnlock, Direction.ClientToServer)]
public sealed partial class ClientItemGenericUnlock
{
    public ItemLocation Location { get; } = new();
}
