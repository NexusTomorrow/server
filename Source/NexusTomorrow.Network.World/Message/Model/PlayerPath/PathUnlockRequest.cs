using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Asks to unlock a path.
/// </summary>
/// <remarks>
/// ActionConfirmButton.UnlockPath sends it, never for a path above 4 or one already unlocked (<see cref="PathUnlockResult.UnlockedPaths"/>).
/// </remarks>
[WireMessage(GameMessageOpcode.Client06BD, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class PathUnlockRequest
{
    /// <summary>
    /// The path to unlock.
    /// </summary>
    [UInt(3)] public Path Path { get; private set; }
}
