using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// If MatchQueueRsult is PersonalSurrenderCooldown or GlobalSurrenderCooldown, uses the WaitTime
[Message(GameMessageOpcode.ServerMatchingMatchOperationResult)]
[WireMessage(GameMessageOpcode.ServerMatchingMatchOperationResult, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchOperationResult : IWritable
{
    [UInt(6)] public MatchingQueueResult Result { get; set; }
    [UInt(32)] public uint WaitTimeBeforeVoteMS { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Result, 6);
        writer.Write(WaitTimeBeforeVoteMS);
    }
}
