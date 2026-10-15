using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

// Triggers MatchVoteKickBegin lua event on client
[Message(GameMessageOpcode.ServerMatchingMatchVoteKickBegin)]
[WireMessage(GameMessageOpcode.ServerMatchingMatchVoteKickBegin, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchVoteKickBegin : IWritable
{
    public Identity Initiator { get; set; } = new();
    public Identity MemberToKick { get; set; } = new();

    public void Write(GamePacketWriter writer)
    {
        Initiator.Write(writer);
        MemberToKick.Write(writer);
    }
}
