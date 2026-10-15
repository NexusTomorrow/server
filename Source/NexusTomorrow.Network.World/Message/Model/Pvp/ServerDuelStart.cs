using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

// Only need to send to the participants.
[Message(GameMessageOpcode.ServerDuelStart)]
[WireMessage(GameMessageOpcode.ServerDuelStart, Direction.ServerToClient)]
public sealed partial class ServerDuelStart : IWritable
{
    [UInt(32)] public uint ChallengerUnitId { get; set; }
    [UInt(32)] public uint OpponentUnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ChallengerUnitId);
        writer.Write(OpponentUnitId);
    }
}
