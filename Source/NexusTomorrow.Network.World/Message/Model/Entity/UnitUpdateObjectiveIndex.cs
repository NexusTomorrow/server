using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Sets the objective index a unit is pursuing, which the client keeps on the unit.
/// </summary>
[WireMessage(GameMessageOpcode.Server093B, Direction.ServerToClient)]
[LayoutConfirmed]
[Verified(Rung.Fields)]
public sealed partial class UnitUpdateObjectiveIndex
{
    /// <summary>
    /// The unit, which the client must not be the player for it to act on.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The objective index the unit is pursuing.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint ObjectiveIndex { get; set; }
}