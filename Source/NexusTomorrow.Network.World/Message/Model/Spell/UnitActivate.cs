using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Activates a unit that has no activation spell.
/// </summary>
/// <remarks>
/// For a unit that has one, the client sends <see cref="UnitActivationCast"/> instead.
/// </remarks>
[WireMessage(GameMessageOpcode.Client00B3, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class UnitActivate
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; private set; }
}
