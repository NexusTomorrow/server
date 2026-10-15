using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

// Similar to ServerSpellThresholdClear but specifically for PressHold and RapidTap cast method spells.
// Explicitly does not affect ChargeRelease spells
[Message(GameMessageOpcode.ServerSpellThresholdCancel)]
[WireMessage(GameMessageOpcode.ServerSpellThresholdCancel, Direction.ServerToClient)]
public sealed partial class ServerSpellThresholdCancel : IWritable
{
    [UInt(18)] public uint SpellId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(SpellId, 18u);
    }
}

