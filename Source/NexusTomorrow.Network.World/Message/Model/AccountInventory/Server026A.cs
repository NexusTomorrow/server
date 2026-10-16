using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server026A, Direction.ServerToClient)]
public sealed partial class Server026A
{
    [WireStruct]
    public sealed partial class Server026APart0
    {
        [UInt(64)] public ulong Unknown0 { get; set; }
        [UInt(64)] public ulong Unknown1 { get; set; }
        [UInt(1)] public bool Unknown2 { get; set; }
        [UInt(64)] public ulong Unknown3 { get; set; }
        [UInt(64)] public ulong Unknown4 { get; set; }
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    [FixedArray(3), UInt(64)] public ulong[] Unknown1 { get; set; } = new ulong[3];
    [UInt(32)] public uint Unknown2 { get; set; }
    [FixedArray(3), UInt(64)] public ulong[] Unknown3 { get; set; } = new ulong[3];
    [Array(CountBits = 32)] public List<Server026APart0> Unknown4 { get; set; } = [];
}
