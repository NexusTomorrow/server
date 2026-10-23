using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Sets a unit's target.
/// </summary>
/// <remarks>
/// The client raises TargetedByUnit or UnTargetedByUnit.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0908, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class UnitTargetChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The target, by unit id.
    /// </summary>
    [UInt(32)] public uint TargetUnitId { get; set; }

    [UInt(32)] public uint Unknown2 { get; set; }
}
