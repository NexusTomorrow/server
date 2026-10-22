using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

[WireMessage(GameMessageOpcode.Server0148, Direction.ServerToClient)]
public sealed partial class ItemRemoved
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(6)] public byte Unknown1 { get; set; }
}
