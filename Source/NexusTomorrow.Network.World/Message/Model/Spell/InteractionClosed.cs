using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Ends the player's interaction with a unit.
/// </summary>
/// <remarks>
/// The client ends the interaction only when it is with that unit, as UnitInteract began it.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0237, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class InteractionClosed
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }
}
