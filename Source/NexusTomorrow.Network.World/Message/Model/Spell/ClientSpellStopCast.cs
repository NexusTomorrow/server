using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ClientSpellStopCast, Direction.ClientToServer)]
public sealed partial class ClientSpellStopCast
{
    [UInt(32)] public uint ServerUniqueId { get; private set; } // first value of 0x7FD response, probably global increment
    [UInt(9)] public CastResult CastResult { get; private set; }
    [UInt(1)] public bool Cancelled { get; private set; } // true = cancelled, false = released
}
