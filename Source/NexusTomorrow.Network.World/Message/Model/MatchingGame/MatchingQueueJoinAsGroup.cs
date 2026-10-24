using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Joins a matching queue as a group; MatchMakingLib.QueueAsGroup sends it.
/// </summary>
[WireMessage(GameMessageOpcode.Client05F3, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class MatchingQueueJoinAsGroup
{
    /// <summary>
    /// The queue to join.
    /// </summary>
    public MatchingQueueRequest Request { get; private set; } = new();

    /// <summary>
    /// The roles queued for, as a mask of <see cref="Role"/>.
    /// </summary>
    [UInt(32)] public Role Roles { get; private set; }

    /// <summary>
    /// The prime level asked for, as <see cref="MatchingQueueJoin.PrimeLevel"/> gives it.
    /// </summary>
    [UInt(32)] public uint PrimeLevel { get; private set; }
}
