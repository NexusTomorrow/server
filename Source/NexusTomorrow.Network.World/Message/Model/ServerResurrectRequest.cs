using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server015B, Direction.ServerToClient)]
public sealed partial class Server015B
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(18)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [Float32] public float Unknown3 { get; set; }
    [Float32] public float Unknown4 { get; set; }
    [Float32] public Vector3 Unknown5 { get; set; }
    [UInt(1)] public byte Unknown6 { get; set; }
}
