using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ClientPublicEventVote)]
[WireMessage(GameMessageOpcode.ClientPublicEventVote, Direction.ClientToServer)]
public sealed partial class ClientPublicEventVote : IReadable
{
    [UInt(14)] public uint EventId { get; private set; }
    [UInt(14)] public uint VoteId { get; private set; }
    [UInt(14)] public uint TeamId { get; private set; }
    [UInt(32)] public uint Choice { get; private set; }

    public void Read(GamePacketReader reader)
    {
        EventId = reader.ReadUInt(14);
        VoteId = reader.ReadUInt(14);
        TeamId = reader.ReadUInt(14);
        Choice = reader.ReadUInt();
    }
}
