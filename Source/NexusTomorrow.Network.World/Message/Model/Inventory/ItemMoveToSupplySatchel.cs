using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

[WireMessage(GameMessageOpcode.Client012A, Direction.ClientToServer)]
public sealed partial class ItemMoveToSupplySatchel
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
    [UInt(32)] public uint Unknown1 { get; private set; }
}
