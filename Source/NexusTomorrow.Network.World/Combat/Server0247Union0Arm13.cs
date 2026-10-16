using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(19)]
public sealed partial class Server0247Union0Arm13 : IServer0247Union0
{
    [UInt(18)] public uint Unknown0 { get; set; }
    [UInt(9)] public ushort Unknown1 { get; set; }
    public Server0247Part0 Unknown2 { get; set; } = new();
}
