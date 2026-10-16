using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server0968, Direction.ServerToClient)]
public sealed partial class Server0968
{
    [WireStruct]
    public sealed partial class Server0968Part0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
    }

    [Array(CountBits = 32)] public List<Server0968Part0> Unknown0 { get; set; } = [];
}
