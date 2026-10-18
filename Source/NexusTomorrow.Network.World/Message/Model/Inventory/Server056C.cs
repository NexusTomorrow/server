using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

[WireMessage(GameMessageOpcode.Server056C, Direction.ServerToClient)]
public sealed partial class Server056C
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
    [UInt(64)] public ulong Unknown2 { get; set; }
    [UInt(18)] public uint Unknown3 { get; set; }
    [Array(CountBits = 3), UInt(32)] public List<uint> Unknown4 { get; } = [];
}
