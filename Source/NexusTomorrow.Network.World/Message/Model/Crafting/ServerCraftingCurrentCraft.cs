using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.Server0854, Direction.ServerToClient)]
public sealed partial class Server0854
{
    [UInt(15)] public uint Unknown0 { get; set; }
    public Client084FPart0 Unknown1 { get; set; } = new Client084FPart0();
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
    [FixedArray(5), UInt(32)] public uint[] Unknown6 { get; set; } = new uint[5];
    [UInt(18)] public uint Unknown7 { get; set; }
    [Float32] public Vector2 Unknown8 { get; set; }
    [Float32] public Vector2 Unknown9 { get; set; }
    [Float32] public float Unknown10 { get; set; }
}
