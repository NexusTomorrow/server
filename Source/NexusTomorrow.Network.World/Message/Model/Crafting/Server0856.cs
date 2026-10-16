using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.Server0856, Direction.ServerToClient)]
public sealed partial class Server0856
{
    [WireStruct]
    public sealed partial class Server0856Part1
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [Float32] public Vector2 Unknown1 { get; set; }
    }

    [Array(CountBits = 32)] public List<Server0856Part0> Unknown0 { get; set; } = [];
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown1 { get; set; } = [];
    [Array(CountBits = 32)] public List<Server0856Part1> Unknown2 { get; set; } = [];
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown3 { get; set; } = [];
    [UInt(32)] public uint Unknown4 { get; set; }
}
