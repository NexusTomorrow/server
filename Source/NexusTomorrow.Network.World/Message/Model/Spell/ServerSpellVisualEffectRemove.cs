using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

// Removes a specific spell visual effect from the game.
[WireMessage(GameMessageOpcode.ServerSpellVisualEffectRemove, Direction.ServerToClient)]
public sealed partial class ServerSpellVisualEffectRemove
{
    [UInt(18)] public uint SpellVisualEffectUniqueId { get; set; }
}

