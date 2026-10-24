using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(27)]
public sealed partial class SetModeCommand : IServer0262Union2
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
