using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

[WireMessage(GameMessageOpcode.Server0140, Direction.ServerToClient)]
public sealed partial class Server0140
{
    [WireStruct]
    public sealed partial class Server0140Part0
    {
        [UInt(8)] public uint Unknown0 { get; set; }
        [UInt(3)] public byte Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }
        [UInt(32)] public uint Unknown5 { get; set; }
    }

    [Array(CountBits = 32)] public List<Server0140Part0> Unknown0 { get; set; } = [];
}
