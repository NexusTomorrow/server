using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Joins a matching queue; MatchMakingLib.Queue sends it.
/// </summary>
[WireMessage(GameMessageOpcode.Client05EF, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class MatchingQueueJoin
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
    /// The prime level asked for. The client lowers it to the smallest ceiling among the prime-level maps queued for, each the
    /// level the player has unlocked there less the map's offset, capped at the map's maximum; it sends 0 when it queues no
    /// prime-level map.
    /// </summary>
    [UInt(32)] public uint PrimeLevel { get; private set; }
}
