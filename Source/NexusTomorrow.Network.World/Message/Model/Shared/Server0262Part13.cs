using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0262Part13
{
    [UInt(16)] public ushort Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(8)] public byte Unknown2 { get; set; }
    public Client0096Part0 Unknown3 { get; set; } = new Client0096Part0();
    [Float32] public float Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
}
