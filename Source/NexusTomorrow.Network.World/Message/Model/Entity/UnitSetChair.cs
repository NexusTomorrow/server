using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Sits a unit in a chair.
/// </summary>
/// <remarks>
/// The client emotes the unit; when the unit is gone it clears the chair only if the clear flag is set.
/// </remarks>
[WireMessage(GameMessageOpcode.Server08CF, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitSetChair
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The chair, by unit id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint ChairUnitId { get; set; }

    /// <summary>
    /// When set, the client clears the chair if the unit is not found.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Clear { get; set; }
}