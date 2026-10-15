using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientItemMoveFromSupplySatchel, Direction.ClientToServer)]
public sealed partial class ClientItemMoveFromSupplySatchel
{
    [UInt(14)] public ushort MaterialId { get; private set; } // 14
    [UInt(32)] public uint Amount { get; private set; }
    [UInt(9)] public ushort Unknown2 { get; private set; } // 9
    [UInt(32)] public uint Unknown3 { get; private set; }
}
