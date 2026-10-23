using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Sets the player's matching eligibility.
/// </summary>
[WireMessage(GameMessageOpcode.Server05B8, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class MatchingEligibilityChanged
{
    /// <summary>
    /// The player's matching eligibility, which the client stores for the player and for the player's group member.
    /// </summary>
    [UInt(32)] public uint Eligibility { get; set; }
}
