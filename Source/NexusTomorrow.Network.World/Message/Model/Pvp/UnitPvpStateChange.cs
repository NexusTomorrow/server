using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

/// <summary>
/// Sets a unit's PvP state, which the client refreshes its moddable inventory for.
/// </summary>
[WireMessage(GameMessageOpcode.Server08BD, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitPvpStateChange
{
    /// <summary>
    /// The unit whose PvP state changes.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The new PvP state, whose bit 0 marks the unit as in PvP.
    /// </summary>
    [VerifiedMember, UInt(3)] public byte PvpState { get; set; }
}