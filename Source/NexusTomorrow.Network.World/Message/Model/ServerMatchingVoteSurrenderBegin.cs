using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// If MatchType is a PvP type, starts a vote to surrender
// If MatchType is a PvE type, starts a vote to disband
[WireMessage(GameMessageOpcode.ServerMatchingMatchVoteSurrenderBegin, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchVoteSurrenderBegin
{
}
