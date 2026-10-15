using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientItemMove, Direction.ClientToServer)]
public sealed partial class ClientItemMove
{
    public ItemLocation From { get; } = new();
    public ItemLocation To { get; } = new();
}
