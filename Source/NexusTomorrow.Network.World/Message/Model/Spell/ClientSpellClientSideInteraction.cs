using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ClientSpellClientSideInteraction, Direction.ClientToServer)]
public sealed partial class ClientSpellClientSideInteraction
{
    [UInt(32)] public uint ServerUniqueId { get; private set; }
    [UInt(3)] public byte CSIResponse { get; private set; } // value of 2 or 0
    [UInt(32)] public uint Spell4IdBaseIdBaseSpell { get; private set; } // Is actually spell4IdBaseIdBaseSpell from spell plus 1, see spell4 tbl
}
