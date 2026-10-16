using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireStruct]
public sealed partial class Client04F5Part0
{
    public Client00B5Part0 Unknown0 { get; } = new();
    [UInt(64)] public ulong Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
    [UInt(16)] public ushort Unknown3 { get; private set; }
    [UInt(16)] public ushort Unknown4 { get; private set; }
    [UInt(32)] public uint Unknown5 { get; private set; }
    [UInt(32)] public uint Unknown6 { get; private set; }
    [UInt(32)] public uint Unknown7 { get; private set; }
    [Float32] public float Unknown8 { get; private set; }
    [Float32] public Vector3 Unknown9 { get; private set; }
    [Float32] public Quaternion Unknown10 { get; private set; }
    [UInt(32)] public uint Unknown11 { get; private set; }
    [UInt(32)] public uint Unknown12 { get; private set; }
    [UInt(64)] public ulong Unknown13 { get; private set; }
    [UInt(14)] public ushort Unknown14 { get; private set; }
}
