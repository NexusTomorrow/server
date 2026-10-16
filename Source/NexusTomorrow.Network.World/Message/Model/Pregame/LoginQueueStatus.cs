using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The login queue's status, which the client passes to the character-select interface.
/// </summary>
/// <remarks>
/// Taken while character select is active; realm select drops it.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0717, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class LoginQueueStatus
{
    /// <summary>
    /// The player's place in the queue, shown as a number.
    /// </summary>
    [UInt(32)] public uint Position { get; set; }

    /// <summary>
    /// The expected wait, in seconds.
    /// </summary>
    [UInt(32)] public uint WaitSeconds { get; set; }

    /// <summary>
    /// Whether the interface shows an extra notice in its queue window.
    /// </summary>
    [UInt(1)] public bool ShowNotice { get; set; }
}
