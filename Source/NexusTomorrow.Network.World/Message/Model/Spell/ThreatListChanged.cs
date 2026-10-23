using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Sets a unit's threat list: up to five units and their threat.
/// </summary>
/// <remarks>
/// The client raises TargetThreatListUpdated.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0909, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ThreatListChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The units on the list, by unit id, the unit's own target first.
    /// </summary>
    [FixedArray(5), UInt(32)] public uint[] UnitIds { get; set; } = new uint[5];

    /// <summary>
    /// Each listed unit's threat, in the same order.
    /// </summary>
    [FixedArray(5), UInt(32)] public uint[] Threats { get; set; } = new uint[5];
}
