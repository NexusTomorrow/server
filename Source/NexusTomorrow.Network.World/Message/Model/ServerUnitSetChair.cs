using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerUnitSetChair, Direction.ServerToClient)]
public sealed partial class ServerUnitSetChair
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(32)] public uint UnitIdChair { get; set; }
    [UInt(1)] public bool WaitForUnit { get; set; }
}
