using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.ClientRewardTrackChooseTrack, Direction.ClientToServer)]
public sealed partial class ClientRewardTrackChooseTrack
{
    [UInt(14)] public ushort RewardTrackId { get; private set; }
}
