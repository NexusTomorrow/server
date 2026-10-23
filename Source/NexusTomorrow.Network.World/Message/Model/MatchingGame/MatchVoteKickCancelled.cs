using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Server060D, Direction.ServerToClient)]
public sealed partial class MatchVoteKickCancelled
{
}
