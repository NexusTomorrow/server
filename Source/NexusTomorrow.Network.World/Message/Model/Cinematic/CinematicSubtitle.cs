using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Shows or clears the cinematic's subtitle at the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server022A, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicSubtitle
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// The subtitle, a LocalizedText id: the client shows its text and plays its voice-over. 0 clears the subtitle.
    /// </summary>
    [UInt(21)] public uint TextId { get; set; }

    [UInt(1)] public bool Unknown2 { get; set; }
}
