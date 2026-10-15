using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class ItemLocation
{
    [UInt(9)] public InventoryLocation Location { get; set; }
    [UInt(32)] public uint BagIndex { get; set; }
}
