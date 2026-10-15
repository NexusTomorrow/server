using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerEntityFaction, Direction.ServerToClient)]
public sealed partial class ServerEntityFaction
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(14)] public Faction Faction { get; set; }
}
