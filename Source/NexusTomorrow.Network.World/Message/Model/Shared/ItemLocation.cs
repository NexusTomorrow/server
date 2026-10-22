using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class ItemLocation
{
    [UInt(9)] public ushort Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
}
