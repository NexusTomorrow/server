using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

// Resets all cooldowns and gives spells infinite charges
[WireMessage(GameMessageOpcode.ServerSpellDisableCooldowns, Direction.ServerToClient)]
public sealed partial class ServerSpellDisableCooldowns
{
    [UInt(1)] public bool DisableCooldowns { get; set; }
}
