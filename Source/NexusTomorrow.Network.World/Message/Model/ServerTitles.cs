using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server018B, Direction.ServerToClient)]
public sealed partial class Server018B
{
    [WireStruct]
    public sealed partial class Server018BPart0
    {
        [UInt(14)] public ushort Unknown0 { get; set; }
        [UInt(1)] public bool Unknown1 { get; set; }
        [UInt(1)] public bool Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
    }

    [Array(CountBits = 32)] public List<Server018BPart0> Unknown0 { get; set; } = [];
}
