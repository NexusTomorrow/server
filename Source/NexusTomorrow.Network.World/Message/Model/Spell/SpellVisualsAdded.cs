using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server0261, Direction.ServerToClient)]
public sealed partial class SpellVisualsAdded
{
    [Array(CountBits = 32)] public List<SpellVisualAdded> Unknown0 { get; set; } = [];
}
