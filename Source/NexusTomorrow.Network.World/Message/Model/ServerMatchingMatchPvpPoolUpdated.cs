using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingMatchPvpPoolUpdated)]
[WireMessage(GameMessageOpcode.ServerMatchingMatchPvpPoolUpdated, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchPvpPoolUpdated : IWritable
{
    [UInt(32)] public uint LivesRemainingTeam1 { get; set; }
    [UInt(32)] public uint LivesRemainingTeam2 { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(LivesRemainingTeam1);
        writer.Write(LivesRemainingTeam2);
    }
}
