using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Tells the client the player is in the match's instance; the client raises MatchEntered.
/// </summary>
[WireMessage(GameMessageOpcode.Server05BF, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchEntered
{
    /// <summary>
    /// The match's map, as a MatchingGameMap id, which the client keeps as the current match's.
    /// </summary>
    [UInt(32)] public uint GameMapId { get; set; }
}
