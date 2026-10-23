using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server04DF, Direction.ServerToClient)]
public sealed partial class HousingResidences
{
    [WireStruct]
    public sealed partial class Residence
    {
        public RealmScopedId Unknown0 { get; set; } = new();
        [UInt(64)] public ulong Unknown1 { get; set; }
        [UInt(64)] public ulong Unknown2 { get; set; }
        [UInt(64)] public ulong Unknown3 { get; set; }
        [UInt(14)] public ushort Unknown4 { get; set; }
        [UInt(32)] public uint Unknown5 { get; set; }
        [WideString] public string Unknown6 { get; set; } = "";
        [UInt(32)] public uint Unknown7 { get; set; }
        [UInt(32)] public uint Unknown8 { get; set; }
        [UInt(32)] public uint Unknown9 { get; set; }
        [UInt(32)] public uint Unknown10 { get; set; }
        [UInt(32)] public uint Unknown11 { get; set; }
        [UInt(32)] public uint Unknown12 { get; set; }
        [UInt(32)] public uint Unknown13 { get; set; }
        [UInt(32)] public uint Unknown14 { get; set; }
        [UInt(32)] public uint Unknown15 { get; set; }
        [UInt(32)] public uint Unknown16 { get; set; }
        [UInt(32)] public uint Unknown17 { get; set; }
        [UInt(32)] public uint Unknown18 { get; set; }
        [FixedArray(8), UInt(64)] public ulong[] Unknown19 { get; set; } = new ulong[8];
        [UInt(1)] public bool Unknown20 { get; set; }
    }

    [Array(CountBits = 32)] public List<Residence> Unknown0 { get; } = [];
}
