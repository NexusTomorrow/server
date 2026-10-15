using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reputation;

// For temporarily overriding a player's reputation value for a specific faction.
// Does not affect the underlying reputation value sent in ServerReputationUpdate.
[Message(GameMessageOpcode.ServerReputationOverrideAdd)]
[WireMessage(GameMessageOpcode.ServerReputationOverrideAdd, Direction.ServerToClient)]
public sealed partial class ServerReputationOverrideAdd : IWritable
{
    [UInt(32)] public uint UnitId { get; set; } // Must be player's UnitId otherwise message is ignored
    [UInt(14)] public Faction FactionId { get; set; }
    [Float32] public float Value { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(FactionId, 14u);
        writer.Write(Value);
    }
}
