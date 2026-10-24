using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Makes the times of the commands that follow count from when the client runs this one.
/// </summary>
[WireMessage(GameMessageOpcode.Server0226, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CinematicTimeRebase
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last rebase it ran; later commands'
    /// times count from it.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }
}
