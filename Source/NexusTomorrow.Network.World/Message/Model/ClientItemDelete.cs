using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientItemDelete, Direction.ClientToServer)]
public sealed partial class ClientItemDelete
{
    public ItemLocation From { get; } = new();
}
