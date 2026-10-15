using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class ItemVisual
{
    [UInt(7)] public ItemSlot Slot { get; set; }
    [UInt(15)] public ushort DisplayId { get; set; }
    [UInt(14)] public ushort ColourSetId { get; set; }
    [UInt(32)] public uint DyeData { get; set; }
}
