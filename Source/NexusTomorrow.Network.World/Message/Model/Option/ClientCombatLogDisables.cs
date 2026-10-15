using NexusTomorrow.Game.Static.Option;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

// Sent whenever one of the options is changed
[WireMessage(GameMessageOpcode.ClientCombatLogDisables, Direction.ClientToServer)]
public sealed partial class ClientCombatLogDisables
{
    [UInt(32)] public CombatLogOptions DisableFlags { get; set; }
}
