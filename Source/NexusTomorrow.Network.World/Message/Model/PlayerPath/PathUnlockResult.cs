using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Reports a path unlock, with the paths unlocked.
/// </summary>
/// <remarks>
/// When <see cref="Unknown0"/> is 0 the client takes <see cref="UnlockedPaths"/> as the player's unlocked paths. It raises
/// PathUnlockResult.
/// </remarks>
[WireMessage(GameMessageOpcode.Server06BE, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class PathUnlockResult
{
    [UInt(8)] public byte Unknown0 { get; set; }

    /// <summary>
    /// The paths unlocked, which the client checks <see cref="PathChangeRequest"/> and <see cref="PathUnlockRequest"/> against.
    /// </summary>
    [UInt(4)] public PathUnlockedMask UnlockedPaths { get; set; }
}
