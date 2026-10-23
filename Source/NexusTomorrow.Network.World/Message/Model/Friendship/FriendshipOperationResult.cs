using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Reports the result of a friendship operation.
/// </summary>
/// <remarks>
/// FriendsList reads the result as eResult. For <see cref="FriendshipResult.ThrottledRequest"/> the client also sends the player's
/// presence again.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03C4, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class FriendshipOperationResult
{
    [WideString] public string Unknown0 { get; set; } = "";

    /// <summary>
    /// The result.
    /// </summary>
    [UInt(6)] public FriendshipResult Result { get; set; }
}
