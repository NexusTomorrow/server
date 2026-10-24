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
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }

    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    [UInt(1)] public bool Unknown4 { get; set; }
}
