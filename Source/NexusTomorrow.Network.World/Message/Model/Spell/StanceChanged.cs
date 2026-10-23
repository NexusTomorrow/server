using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Sets the player's stance, the index of the class's innate ability.
/// </summary>
/// <remarks>
/// The client raises StanceChanged.
/// </remarks>
[WireMessage(GameMessageOpcode.Server019F, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class StanceChanged
{
    /// <summary>
    /// The innate ability's index.
    /// </summary>
    [UInt(2)] public byte Stance { get; set; }
}
