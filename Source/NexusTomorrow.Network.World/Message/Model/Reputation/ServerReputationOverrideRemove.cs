using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reputation;

[Message(GameMessageOpcode.ServerReputationOverrideRemove)]
[WireMessage(GameMessageOpcode.ServerReputationOverrideRemove, Direction.ServerToClient)]
public sealed partial class ServerReputationOverrideRemove : IWritable
{
    [UInt(32)] public uint UnitId { get; set; } // Must be player's UnitId otherwise message is ignored
    [UInt(14)] public Faction FactionId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(FactionId, 14u);
    }
}
