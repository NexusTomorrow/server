using NexusTomorrow.Game.Static.Challenges;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Challenges;

[Message(GameMessageOpcode.ServerChallengeResult)]
[WireMessage(GameMessageOpcode.ServerChallengeResult, Direction.ServerToClient)]
public sealed partial class ServerChallengeResult : IWritable
{
    [UInt(14)] public ushort ChallengeId { get; set; }
    [UInt(32)] public ChallengeResult Result { get; set; }
    [Pad(32)] public Pad ResultHigh { get; }
    [UInt(32)] public int Data { get; set; } // Sometimes localizedStringId, sometimes tier value

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ChallengeId, 14u);
        writer.Write(Result);
        writer.Write(Data);
    }
}
