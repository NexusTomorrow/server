using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

// Triggers MatchVoteKickBegin lua event on client
[WireMessage(GameMessageOpcode.ServerMatchingMatchVoteKickBegin, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchVoteKickBegin
{
    public Identity Initiator { get; set; } = new();
    public Identity MemberToKick { get; set; } = new();
}
