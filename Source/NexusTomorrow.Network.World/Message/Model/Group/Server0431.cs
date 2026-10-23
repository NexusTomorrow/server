using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Server0431, Direction.ServerToClient)]
public sealed partial class Server0431
{
    [WireStruct]
    public sealed partial class Server0431Part0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
    }

    [UInt(64)] public ulong Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<Server0431Part0> Unknown1 { get; } = [];
}
