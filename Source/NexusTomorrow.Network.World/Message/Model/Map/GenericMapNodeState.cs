using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[WireStruct]
public sealed partial class GenericMapNodeState
{
    [UInt(14)] public uint Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
}
