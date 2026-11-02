using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Destroys a unit.
/// </summary>
[WireMessage(GameMessageOpcode.Server0355, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitDestroyed
{
    /// <summary>
    /// The unit the client removes, by unit id: once this names the id of a unit the client holds, the unit is gone from its
    /// unit lookup, and it stays when this names another id. An id the client does not hold is ignored.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// For a player unit, when set the client marks the unit's destroyed state before raising UnitDestroyed; ignored for other units.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool DestroyFlag { get; set; }
}
