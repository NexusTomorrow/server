using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

// Resets all cooldowns and gives spells infinite charges
[Message(GameMessageOpcode.ServerSpellDisableCooldowns)]
[WireMessage(GameMessageOpcode.ServerSpellDisableCooldowns, Direction.ServerToClient)]
public sealed partial class ServerSpellDisableCooldowns : IWritable
{
    [UInt(1)] public bool DisableCooldowns { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(DisableCooldowns);
    }
}
