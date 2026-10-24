using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0262Part12
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(8)] public byte Unknown1 { get; set; }
    public Client0096Part0 Unknown2 { get; set; } = new Client0096Part0();
    [Float32] public float Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
}
