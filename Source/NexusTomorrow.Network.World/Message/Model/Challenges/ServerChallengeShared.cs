using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Challenges;

[Message(GameMessageOpcode.ServerChallengeShared)]
[WireMessage(GameMessageOpcode.ServerChallengeShared, Direction.ServerToClient)]
public sealed partial class ServerChallengeShared : IWritable
{
    [UInt(14)] public ushort ChallengeId { get; set; }
    [UInt(32)] public uint SharerUnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ChallengeId, 14u);
        writer.Write(SharerUnitId);
    }
}
