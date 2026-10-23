using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Asks to change the player's active path.
/// </summary>
/// <remarks>
/// ActionConfirmButton.ChangePath sends it, never for the active path or a path not unlocked (<see cref="PathUnlockResult.UnlockedPaths"/>).
/// The server answers with <see cref="PathChangeResult"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client06B2, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class PathChangeRequest
{
    /// <summary>
    /// The path to change to.
    /// </summary>
    [UInt(3)] public Path Path { get; private set; }

    [UInt(1)] public bool Unknown1 { get; private set; }
}
