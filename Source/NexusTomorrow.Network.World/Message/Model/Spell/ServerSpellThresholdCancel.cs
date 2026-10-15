using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

// Similar to ServerSpellThresholdClear but specifically for PressHold and RapidTap cast method spells.
// Explicitly does not affect ChargeRelease spells
[WireMessage(GameMessageOpcode.ServerSpellThresholdCancel, Direction.ServerToClient)]
public sealed partial class ServerSpellThresholdCancel
{
    [UInt(18)] public uint SpellId { get; set; }
}

