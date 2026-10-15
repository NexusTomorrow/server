using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Combat;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerCombatLog, Direction.ServerToClient)]
public partial class ServerCombatLog
{
    [UInt(6)] public CombatLogType Type { get; set; }
    [Union(nameof(Type))] public ICombatLog CombatLog { get; set; }
}
