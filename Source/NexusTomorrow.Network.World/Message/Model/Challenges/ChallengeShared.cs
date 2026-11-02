using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Challenges;

/// <summary>
/// Announces a shared challenge.
/// </summary>
/// <remarks>
/// The client raises ChallengeShared with the challenge id and target.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00A9, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ChallengeShared
{
    /// <summary>
    /// The challenge, by challenge id.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort ChallengeId { get; set; }

    /// <summary>
    /// The challenge's target, by unit id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint TargetUnitId { get; set; }
}