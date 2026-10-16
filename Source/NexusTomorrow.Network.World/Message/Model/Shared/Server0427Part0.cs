using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0427Part0
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32), Count] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }

    [UInt(3)] public byte Unknown4 { get; set; }
    [UInt(3)] public byte Unknown5 { get; set; }
    [UInt(4)] public byte Unknown6 { get; set; }
    [UInt(2)] public byte Unknown7 { get; set; }
    [Array(CountFrom = nameof(Unknown2))] public List<Server0405Part0> Unknown8 { get; set; } = [];

    public Client00B5Part0 Unknown9 { get; set; } = new();
    [UInt(14)] public ushort Unknown10 { get; set; }

    public Server0427Part1 Unknown11 { get; set; }
}
