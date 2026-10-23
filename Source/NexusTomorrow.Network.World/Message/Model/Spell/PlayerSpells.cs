using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server0551, Direction.ServerToClient)]
public sealed partial class PlayerSpells
{
    [Array(CountBits = 32)] public List<Server0262Part7> Unknown0 { get; } = [];
}

