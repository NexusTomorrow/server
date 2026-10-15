using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reputation;

[WireMessage(GameMessageOpcode.ServerReputationOverrideRemove, Direction.ServerToClient)]
public sealed partial class ServerReputationOverrideRemove
{
    [UInt(32)] public uint UnitId { get; set; } // Must be player's UnitId otherwise message is ignored
    [UInt(14)] public Faction FactionId { get; set; }
}
