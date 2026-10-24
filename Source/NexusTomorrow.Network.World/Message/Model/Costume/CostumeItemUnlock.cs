using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[WireMessage(GameMessageOpcode.Client090F, Direction.ClientToServer)]
public sealed partial class CostumeItemUnlock
{
    public ItemLocation Unknown0 { get; } = new();
}
