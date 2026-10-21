using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireStruct]
public sealed partial class Server0487Part0
{
    public RealmScopedId Unknown0 { get; set; } = new();
    [UInt(64)] public ulong Unknown1 { get; set; }
    [UInt(17)] public uint Unknown2 { get; set; }
    [UInt(64)] public ulong Unknown3 { get; set; }
    [UInt(64)] public ulong Unknown4 { get; set; }
    [UInt(64)] public ulong Unknown5 { get; set; }
    [UInt(64)] public ulong Unknown6 { get; set; }
    [Float32] public float Unknown7 { get; set; }
}
