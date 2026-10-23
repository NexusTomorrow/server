using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Sets the player's matching penalties; the client raises MatchingPenaltyUpdated.
/// </summary>
[WireMessage(GameMessageOpcode.Server05D9, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class MatchingPenalties
{
    /// <summary>
    /// The player's sixteen penalty values, which the client stores as they come.
    /// </summary>
    [FixedArray(16), UInt(32)] public uint[] Penalties { get; set; } = new uint[16];
}
