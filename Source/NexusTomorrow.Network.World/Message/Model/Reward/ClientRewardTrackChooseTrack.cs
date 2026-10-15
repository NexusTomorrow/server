using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[Message(GameMessageOpcode.ClientRewardTrackChooseTrack)]
[WireMessage(GameMessageOpcode.ClientRewardTrackChooseTrack, Direction.ClientToServer)]
public sealed partial class ClientRewardTrackChooseTrack : IReadable
{
    [UInt(14)] public ushort RewardTrackId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        RewardTrackId = reader.ReadUShort(14u);
    }
}
