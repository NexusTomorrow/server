using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Reports the result of a match operation.
/// </summary>
/// <remarks>
/// On <see cref="MatchingQueueResult.PersonalSurrenderCooldown"/> or <see cref="MatchingQueueResult.GlobalSurrenderCooldown"/> the
/// client keeps the result and the cooldown, and holds back <see cref="MatchVoteSurrenderInitiate"/> until the cooldown has run out;
/// on any other result it prints that result's text.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0623, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchOperationResult
{
    /// <summary>
    /// The result.
    /// </summary>
    [UInt(6)] public MatchingQueueResult Result { get; set; }

    /// <summary>
    /// With a surrender cooldown result, the cooldown in milliseconds, which the client counts from the message's arrival.
    /// </summary>
    [UInt(32)] public uint CooldownMs { get; set; }
}
