using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class InventoryItem : IWritable
{
    public Item Item { get; set; }
    [UInt(6)] public ItemUpdateReason Reason { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Item.Write(writer);
        writer.Write(Reason, 6u);
    }
}
