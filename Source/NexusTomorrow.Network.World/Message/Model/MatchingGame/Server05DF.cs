using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Server05DF, Direction.ServerToClient)]
public sealed partial class Server05DF
{
    [WireStruct]
    public sealed partial class Server05DFPart0
    {
        [UInt(5)] public byte Unknown0 { get; set; }
        [Array(CountBits = 32), UInt(32)] public List<uint> Unknown1 { get; set; } = [];
        [UInt(14)] public ushort Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
    }

    [WireStruct]
    public sealed partial class Server05DFPart1
    {
        [UInt(5)] public byte Unknown0 { get; set; }
        [UInt(1)] public bool Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
    }

    public Server05DFPart0 Unknown0 { get; set; } = new();
    public Server05DFPart1 Unknown1 { get; set; } = new();
    [UInt(32)] public uint Unknown2 { get; set; }
}
