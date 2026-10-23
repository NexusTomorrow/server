using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Stops one of the player's casts, with a reason.
/// </summary>
[WireMessage(GameMessageOpcode.Client0801, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class SpellCastStop
{
    [UInt(32)] public uint Unknown0 { get; private set; }

    /// <summary>
    /// The reason, a Spell4CastResult table id.
    /// </summary>
    [UInt(9)] public ushort Result { get; private set; }

    [UInt(1)] public bool Unknown2 { get; private set; }
}
