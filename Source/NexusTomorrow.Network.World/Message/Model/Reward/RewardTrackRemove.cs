using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

/// <summary>
/// Removes a reward track by id, or all tracks when the id is 0.
/// </summary>
[WireMessage(GameMessageOpcode.Server07D0, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class RewardTrackRemove
{
    /// <summary>
    /// The reward track to remove, by id; 0 removes all tracks.
    /// </summary>
    [VerifiedMember, UInt(14)] public uint RewardTrackId { get; set; }
}
