using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

[WireMessage(GameMessageOpcode.Server056B, Direction.ServerToClient)]
public sealed partial class Server056B
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(64)] public ulong Unknown3 { get; set; }
}
