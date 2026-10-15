using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class InventoryItem
{
    public Item Item { get; set; }
    [UInt(6)] public ItemUpdateReason Reason { get; set; }
}
