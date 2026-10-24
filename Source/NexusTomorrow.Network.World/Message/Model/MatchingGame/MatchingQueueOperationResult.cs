using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Reports the result of a matching queue operation.
/// </summary>
/// <remarks>
/// The client prints the result's text. On <see cref="MatchingQueueResult.RemovedByServer"/> it raises MatchLeft, on
/// <see cref="MatchingQueueResult.NotConfirmingRole"/> MatchingRoleCheckCanceled, and on
/// <see cref="MatchingQueueResult.NotWaitingToAccept"/> MatchingCancelPendingGame.
/// </remarks>
[WireMessage(GameMessageOpcode.Server05E1, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class MatchingQueueOperationResult
{
    /// <summary>
    /// The result.
    /// </summary>
    [UInt(6)] public MatchingQueueResult Result { get; set; }

    [UInt(4)] public byte Unknown1 { get; set; }
}
