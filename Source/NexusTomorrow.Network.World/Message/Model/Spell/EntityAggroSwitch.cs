using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Tells the client a unit switches its aggro to the player.
/// </summary>
[WireMessage(GameMessageOpcode.Server0876, Direction.ServerToClient)]
[LayoutConfirmed]
[Verified(Rung.Fields)]
public sealed partial class EntityAggroSwitch
{
    /// <summary>
    /// The unit that switches its aggro.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The player the unit switches aggro to; the client acts on it only when it is the client's own player.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint PlayerId { get; set; }
}
