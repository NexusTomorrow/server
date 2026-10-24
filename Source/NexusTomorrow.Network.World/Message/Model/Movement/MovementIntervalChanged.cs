using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.WorldEntry;

namespace NexusTomorrow.Network.World.Message.Model.Movement;

/// <summary>
/// Changes the period of the client's movement timer.
/// </summary>
/// <remarks>
/// The client takes the new period at once, as it takes <see cref="WorldSettings.MovementInterval"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0070, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MovementIntervalChanged
{
    /// <summary>
    /// The period, in milliseconds, of the client's movement timer, the value <see cref="WorldSettings.MovementInterval"/> also sets: the
    /// client sends the movement of the unit it moves (<see cref="ClientMovement"/>) on it, and fits a sender's clock by it
    /// (<see cref="UnitMovement.Time"/>). The client clamps it to 25 to 1,000.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint MovementInterval { get; set; }
}
