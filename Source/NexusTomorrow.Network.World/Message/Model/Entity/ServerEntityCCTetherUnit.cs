using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

// If a Tether CCState is applied to the player, the TetherUnit is the unit from which the tether length is calculated.
[Message(GameMessageOpcode.ServerEntityCCTetherUnit)]
[WireMessage(GameMessageOpcode.ServerEntityCCTetherUnit, Direction.ServerToClient)]
public sealed partial class ServerEntityCCTetherUnit : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
    }
}
