using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server04E1, Direction.ServerToClient)]
public sealed partial class Server04E1
{
    [WireStruct]
    public sealed partial class Server04E1Part0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
        [FixedArray(5), UInt(32)] public uint[] Unknown4 { get; } = new uint[5];
        [FixedArray(5), UInt(32)] public uint[] Unknown5 { get; } = new uint[5];
        [UInt(32)] public uint Unknown6 { get; set; }
        [Float32] public float Unknown7 { get; set; }
        [UInt(3)] public byte Unknown8 { get; set; }
        [UInt(32)] public uint Unknown9 { get; set; }
        [Float32] public float Unknown10 { get; set; }
        [Float32] public float Unknown11 { get; set; }

        public Server04E1Part0()
        {
            Array.Fill(Unknown4, 0u);
            Array.Fill(Unknown5, 0u);
        }
    }

    public Client00B5Part0 Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<Server04E1Part0> Unknown1 { get; set; } = [];
}
