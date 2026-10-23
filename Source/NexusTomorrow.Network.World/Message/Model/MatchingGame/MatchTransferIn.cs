using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Asks to be moved into the current match; the client sends it only when the match's type allows it.
/// </summary>
[WireMessage(GameMessageOpcode.Client0606, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class MatchTransferIn
{
}
