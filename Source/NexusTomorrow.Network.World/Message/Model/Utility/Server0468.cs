using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[WireMessage(GameMessageOpcode.Server0468, Direction.ServerToClient)]
public sealed partial class Server0468
{
    [WireStruct]
    public sealed partial class Server0468Part0
    {
        [UInt(15)] public ushort Unknown0 { get; set; }
        [UInt(16)] public ushort Unknown1 { get; set; }
    }

    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(14)] public ushort Unknown1 { get; set; }
    [UInt(64)] public ulong Unknown2 { get; set; }
    [Array(CountBits = 32)] public List<Server0468Part0> Unknown3 { get; } = [];
}
