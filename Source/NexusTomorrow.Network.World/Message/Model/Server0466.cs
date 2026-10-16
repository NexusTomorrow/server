using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0466, Direction.ServerToClient)]
public sealed partial class Server0466
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    public Client00B5Part0 Unknown1 { get; set; } = new();

    [UInt(7)] public byte Unknown2 { get; set; }
    [UInt(7)] public byte Unknown3 { get; set; }

    [UInt(17)] public uint Unknown4 { get; set; }
    [UInt(16)] public ushort Unknown5 { get; set; }

    [FixedArray(5)] public Server0405Part1.Server0405Part2[] Unknown6 { get; set; } = [new(), new(), new(), new(), new()];

    [PackedFloat] public float Unknown7 { get; set; }
    [PackedFloat] public float Unknown8 { get; set; }
    [PackedFloat] public float Unknown9 { get; set; }
    [PackedFloat] public float Unknown10 { get; set; }
    [PackedFloat] public float Unknown11 { get; set; }
    [PackedFloat] public float Unknown12 { get; set; }
    [PackedFloat] public float Unknown13 { get; set; }
    [PackedFloat] public float Unknown14 { get; set; }
    [PackedFloat] public float Unknown15 { get; set; }
    [PackedFloat] public float Unknown16 { get; set; }
    [PackedFloat] public float Unknown17 { get; set; }
    [PackedFloat] public float Unknown18 { get; set; }

    [UInt(32)] public uint Unknown19 { get; set; }
    [UInt(32)] public uint Unknown20 { get; set; }
    [UInt(3)] public byte Unknown21 { get; set; }
}
