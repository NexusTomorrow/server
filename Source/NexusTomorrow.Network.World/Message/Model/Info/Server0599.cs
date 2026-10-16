using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[WireMessage(GameMessageOpcode.Server0599, Direction.ServerToClient)]
public sealed partial class Server0599
{
    public Server0598Part0 Unknown0 { get; set; } = new();
    [UInt(1)] public bool Unknown1 { get; set; }
    [UInt(3)] public byte Unknown2 { get; set; }
    [UInt(8)] public byte Unknown3 { get; set; }
    [UInt(6)] public byte Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(1)] public bool Unknown6 { get; set; }
    [Float32] public float Unknown7 { get; set; }
}
