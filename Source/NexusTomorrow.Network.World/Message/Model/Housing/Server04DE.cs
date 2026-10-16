using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server04DE, Direction.ServerToClient)]
public sealed partial class Server04DE
{
    [WireStruct]
    public sealed partial class Server04DEPart0
    {
        public Client00B5Part0 Unknown0 { get; set; } = new();
        [UInt(64)] public ulong Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }
        [UInt(32)] public uint Unknown5 { get; set; }
        [UInt(32)] public uint Unknown6 { get; set; }
        [Float32] public float Unknown7 { get; set; }
        [Float32] public Vector3 Unknown8 { get; set; } = new();
        [Float32] public Quaternion Unknown9 { get; set; } = new();
        [UInt(32)] public uint Unknown10 { get; set; }
        [UInt(32)] public uint Unknown11 { get; set; }
        [UInt(64)] public ulong Unknown12 { get; set; }
        [UInt(14)] public ushort Unknown13 { get; set; }
    }
    [UInt(32), Count] public uint Unknown0 { get; set; }

    [UInt(32)] public uint Unknown1 { get; set; }
    [Array(CountFrom = nameof(Unknown0))] public List<Server04DEPart0> Unknown2 { get; set; } = [];
}
