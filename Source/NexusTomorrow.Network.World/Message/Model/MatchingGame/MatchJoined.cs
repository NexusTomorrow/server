using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Tells the client the player joined a match; the client clears the role check and the pending match and raises MatchJoined.
/// </summary>
[WireMessage(GameMessageOpcode.Server05C6, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchJoined
{
    /// <summary>
    /// The match's map, as a MatchingGameMap id, which the client keeps as the current match's.
    /// </summary>
    [UInt(14)] public uint GameMapId { get; set; }
}
