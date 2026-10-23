using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Cancels an aura the player owns.
/// </summary>
/// <remarks>
/// The client sends it only for an aura that can be canceled.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0802, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class SpellCancel
{
    /// <summary>
    /// The aura's cast, by the server's id for it.
    /// </summary>
    [UInt(32)] public uint CastId { get; private set; }
}
