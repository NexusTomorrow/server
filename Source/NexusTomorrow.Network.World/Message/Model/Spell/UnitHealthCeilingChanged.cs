using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Sets a unit's health ceiling.
/// </summary>
/// <remarks>
/// The client raises RefreshHealthCeiling.
/// </remarks>
[WireMessage(GameMessageOpcode.Server093F, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitHealthCeilingChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The unit's health ceiling.
    /// </summary>
    [UInt(32)] public uint HealthCeiling { get; set; }
}
