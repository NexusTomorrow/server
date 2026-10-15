using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientMatchingMatchInitiateVoteToKick)]
[WireMessage(GameMessageOpcode.ClientMatchingMatchInitiateVoteToKick, Direction.ClientToServer)]
public sealed partial class ClientMatchingMatchInitiateVoteToKick : IReadable
{
    public Identity MemberToKick { get; private set; } = new();

    public void Read(GamePacketReader reader)
    {
        MemberToKick.Read(reader);
    }
}
