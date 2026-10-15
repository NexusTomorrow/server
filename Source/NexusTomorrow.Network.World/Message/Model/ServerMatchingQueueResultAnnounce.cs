using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingQueueResultAnnounce)]
[WireMessage(GameMessageOpcode.ServerMatchingQueueResultAnnounce, Direction.ServerToClient)]
public sealed partial class ServerMatchingQueueResultAnnounce : IWritable
{
    [UInt(6)] public MatchingQueueResult Result { get; set; }
    [UInt(4)] public MatchingQueueStatus Status { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Result, 6u);
        writer.Write(Status, 4u);
    }
}
