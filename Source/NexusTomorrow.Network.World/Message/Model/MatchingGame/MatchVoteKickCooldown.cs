using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Has the client print the kick cooldown in the matching text, which it does only for two values of <see cref="Result"/>.
/// </summary>
[WireMessage(GameMessageOpcode.Server0616, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchVoteKickCooldown
{
    /// <summary>
    /// The match queue result; the client prints the cooldown only for the kick-cooldown values.
    /// </summary>
    [VerifiedMember, UInt(6)] public byte Result { get; set; }

    /// <summary>
    /// The kick cooldown in milliseconds, which the client displays as a seconds time.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint CooldownMs { get; set; }
}
