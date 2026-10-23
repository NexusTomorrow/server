using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

[WireMessage(GameMessageOpcode.Client014C, Direction.ClientToServer)]
public sealed partial class VendorRepair
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
    [UInt(64)] public ulong Unknown2 { get; set; }
}
