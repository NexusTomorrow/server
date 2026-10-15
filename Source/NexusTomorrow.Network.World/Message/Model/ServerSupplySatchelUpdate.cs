using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;


[WireMessage(GameMessageOpcode.ServerSupplySatchelUpdate, Direction.ServerToClient)]
public sealed partial class ServerSupplySatchelUpdate
{
    [UInt(14)] public ushort MaterialId { get; set; }
    [UInt(16)] public ushort StackCount { get; set; }
}
