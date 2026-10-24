using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class ItemUpdate
{
    public ItemInstance Unknown0 { get; set; } = new();
    [UInt(6)] public byte Unknown1 { get; set; }
}
