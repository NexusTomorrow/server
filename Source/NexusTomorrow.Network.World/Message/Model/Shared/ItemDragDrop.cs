using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class ItemDragDrop
{
    [UInt(64)] public ulong Guid { get; set; }
    [UInt(64)] public ulong DragDrop { get; set; }
}
