using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Sets the player's target.
/// </summary>
/// <remarks>
/// The client sets the target itself before sending, and raises TargetUnitChanged.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0185, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class TargetSelect
{
    /// <summary>
    /// The target, by unit id; 0 clears the target.
    /// </summary>
    [UInt(32)] public uint UnitId { get; private set; }
}
