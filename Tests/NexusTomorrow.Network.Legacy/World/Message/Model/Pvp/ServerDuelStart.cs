using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

// Only need to send to the participants.
[Message(GameMessageOpcode.ServerDuelStart)]
public class ServerDuelStart : IWritable
{
    public uint ChallengerUnitId { get; set; }
    public uint OpponentUnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ChallengerUnitId);
        writer.Write(OpponentUnitId);
    }
}
