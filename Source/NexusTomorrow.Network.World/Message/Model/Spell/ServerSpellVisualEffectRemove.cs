using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

// Removes a specific spell visual effect from the game.
[Message(GameMessageOpcode.ServerSpellVisualEffectRemove)]
[WireMessage(GameMessageOpcode.ServerSpellVisualEffectRemove, Direction.ServerToClient)]
public sealed partial class ServerSpellVisualEffectRemove : IWritable
{
    [UInt(18)] public uint SpellVisualEffectUniqueId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(SpellVisualEffectUniqueId, 18u);
    }
}

