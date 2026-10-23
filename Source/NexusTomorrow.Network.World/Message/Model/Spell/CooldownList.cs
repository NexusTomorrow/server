using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server091B, Direction.ServerToClient)]
public sealed partial class CooldownList
{
    [Array(CountBits = 32)] public List<SpellCooldown> Unknown0 { get; set; } = [];
}
