using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Binding
{
    [UInt(14)] public ushort InputActionId { get; set; }
    [UInt(32)] public uint DeviceEnum00 { get; set; }
    [UInt(32)] public uint DeviceEnum01 { get; set; }
    [UInt(32)] public uint DeviceEnum02 { get; set; }
    [UInt(32)] public uint Code00 { get; set; }
    [UInt(32)] public uint Code01 { get; set; }
    [UInt(32)] public uint Code02 { get; set; }
    [UInt(32)] public uint MetaKeys00 { get; set; }
    [UInt(32)] public uint MetaKeys01 { get; set; }
    [UInt(32)] public uint MetaKeys02 { get; set; }
    [UInt(32)] public uint EventTypeEnum00 { get; set; }
    [UInt(32)] public uint EventTypeEnum01 { get; set; }
    [UInt(32)] public uint EventTypeEnum02 { get; set; }
}
