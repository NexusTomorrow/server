using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[Message(GameMessageOpcode.ClientRewardTrackChooseTrack)]
public class ClientRewardTrackChooseTrack : IReadable
{
    public ushort RewardTrackId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        RewardTrackId = reader.ReadUShort(14u);
    }
}
