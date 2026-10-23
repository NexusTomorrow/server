using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Sets the health of a unit other than the player.
/// </summary>
/// <remarks>
/// The client raises RefreshHealthShieldBar.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0937, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitHealthChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The unit's health.
    /// </summary>
    [UInt(32)] public uint Health { get; set; }

    /// <summary>
    /// True when the unit is dead; the client reads it only when the health is 0.
    /// </summary>
    [UInt(1)] public bool IsDead { get; set; }
}
