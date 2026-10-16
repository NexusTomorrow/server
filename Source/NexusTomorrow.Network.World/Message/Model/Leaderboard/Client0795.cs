using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[WireMessage(GameMessageOpcode.Client0795, Direction.ClientToServer)]
public sealed partial class Client0795
{
    [TerminatedWideString] public string Unknown0 { get; set; } = "";
    [TerminatedWideString] public string Unknown1 { get; set; } = "";
    [UInt(32)] public uint Unknown2 { get; set; }
    [TerminatedWideString] public string Unknown3 { get; set; } = "";
    [WideString] public string Unknown4 { get; set; } = "";
    [WideString] public string Unknown5 { get; set; } = "";
    [WideString] public string Unknown6 { get; set; } = "";
    [UInt(32)] public uint Unknown7 { get; set; }
    [UInt(32)] public uint Unknown8 { get; set; }
    [UInt(32)] public uint Unknown9 { get; set; }
    [UInt(32)] public uint Unknown10 { get; set; }
    [UInt(32)] public uint Unknown11 { get; set; }
    [UInt(32)] public uint Unknown12 { get; set; }
    [WideString] public string Unknown13 { get; set; } = "";
    [UInt(32)] public uint Unknown14 { get; set; }
    [UInt(32)] public uint Unknown15 { get; set; }
    [UInt(32)] public uint Unknown16 { get; set; }
    [UInt(32)] public uint Unknown17 { get; set; }
    [UInt(32)] public uint Unknown18 { get; set; }
    [UInt(32)] public uint Unknown19 { get; set; }
    [UInt(32)] public uint Unknown20 { get; set; }
    [UInt(32)] public uint Unknown21 { get; set; }
    [UInt(32)] public uint Unknown22 { get; set; }
}
