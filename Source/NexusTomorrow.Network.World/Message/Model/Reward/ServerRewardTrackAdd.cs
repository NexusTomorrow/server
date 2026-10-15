using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.ServerRewardTrackAdd, Direction.ServerToClient)]
public sealed partial class ServerRewardTrackAdd
{
    public RewardTrack RewardTrack { get; set; }
}
