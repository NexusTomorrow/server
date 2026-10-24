using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Attaches the cinematic camera's position to a unit, from the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server0213, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicCameraAttach
{
    /// <summary>
    /// Which of the unit's model lookups <see cref="AttachId"/> is resolved through.
    /// </summary>
    [UInt(32)] public uint AttachKind { get; set; }

    /// <summary>
    /// The attachment on the unit's model, which <see cref="AttachKind"/>'s lookup resolves.
    /// </summary>
    [UInt(32)] public uint AttachId { get; set; }

    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// Makes the camera take the attachment's facing.
    /// </summary>
    [UInt(1)] public bool UseAttachFacing { get; set; }
}
