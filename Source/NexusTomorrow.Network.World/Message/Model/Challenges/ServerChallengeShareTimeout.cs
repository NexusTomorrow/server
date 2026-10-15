using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Challenges;

// Share timeout is set at 30 seconds per GameFormulaId 0x416
[Message(GameMessageOpcode.ServerChallengeShareTimeout)]
[WireMessage(GameMessageOpcode.ServerChallengeShareTimeout, Direction.ServerToClient)]
public sealed partial class ServerChallengeShareTimeout : IWritable
{
    [UInt(14)] public ushort ChallengeId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ChallengeId, 14u);
    }
}
