using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerUnitEnteredCombat, Direction.ServerToClient)]
public sealed partial class ServerUnitEnteredCombat
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(1)] public bool InCombat { get; set; }
}
