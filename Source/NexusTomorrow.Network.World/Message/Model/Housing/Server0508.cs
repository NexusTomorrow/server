using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server0508, Direction.ServerToClient)]
public sealed partial class Server0508
{
    [WireStruct]
    public sealed partial class Server0508Part0
    {
        [UInt(64)] public ulong Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
    }

    [Array(CountBits = 32)] public List<Server0508Part0> Unknown0 { get; set; } = [];
    [UInt(2)] public byte Unknown1 { get; set; }
}
