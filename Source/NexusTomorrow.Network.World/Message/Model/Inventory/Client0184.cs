using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

[WireMessage(GameMessageOpcode.Client0184, Direction.ClientToServer)]
public sealed partial class Client0184
{
    [UInt(14)] public ushort Unknown0 { get; private set; }
    [UInt(32)] public uint Unknown1 { get; private set; }
    [UInt(9)] public ushort Unknown2 { get; private set; }
    [UInt(32)] public uint Unknown3 { get; private set; }
}
