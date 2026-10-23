using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Tells the client the player left the match; the client resets its match state and raises MatchLeft.
/// </summary>
[WireMessage(GameMessageOpcode.Server05DC, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchLeft
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(5)] public byte Unknown0 { get; set; }
}
