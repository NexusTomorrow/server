using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingQueueStatus)]
[WireMessage(GameMessageOpcode.ServerMatchingQueueStatus, Direction.ServerToClient)]
public sealed partial class ServerMatchingQueueStatus : IWritable
{
    [UInt(4)] public MatchingQueueStatus Status { get; set; }
    [UInt(5)] public Game.Static.Matching.MatchType JoinedMatchType { get; set; }
    [UInt(5)] public Game.Static.Matching.MatchType ReadyMatchType { get; set; } // MatchType for active role check on group that members can role check for
    [FixedArray(2), UInt(8)] public byte[] QueuesJoined { get; set; } = new byte[2];
    // QueuesJoined is array of bits to indicate which MatchType queue is joined, true = joined false = not joined
    // Order of array is same order as Game.Static.Matching.MatchType enum, least significant bit of each byte first

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Status, 4u);
        writer.Write(JoinedMatchType, 5u);
        writer.Write(ReadyMatchType, 5u);
        writer.WriteBytes(QueuesJoined);
    }
}
