using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerUnitInUse, Direction.ServerToClient)]
public sealed partial class ServerUnitInUse
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(1)] public bool InUse { get; set; }
}
