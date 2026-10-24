using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Hides the story panel at the command's time; the client raises StoryPanelDialog_Hide.
/// </summary>
[WireMessage(GameMessageOpcode.Server0229, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CinematicStoryPanelHide
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }
}
