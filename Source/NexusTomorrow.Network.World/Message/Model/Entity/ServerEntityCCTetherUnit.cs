using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

// If a Tether CCState is applied to the player, the TetherUnit is the unit from which the tether length is calculated.
[WireMessage(GameMessageOpcode.ServerEntityCCTetherUnit, Direction.ServerToClient)]
public sealed partial class ServerEntityCCTetherUnit
{
    [UInt(32)] public uint UnitId { get; set; }
}
