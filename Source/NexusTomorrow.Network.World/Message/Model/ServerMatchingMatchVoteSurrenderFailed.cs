using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// If MatchType is a PvP type, vote was to surrender
// If MatchType is a PvE type, vote was to disband
[WireMessage(GameMessageOpcode.ServerMatchingMatchVoteSurrenderFailed, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchVoteSurrenderFailed
{
}
