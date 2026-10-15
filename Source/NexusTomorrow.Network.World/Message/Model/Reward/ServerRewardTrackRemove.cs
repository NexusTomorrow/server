using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.ServerRewardTrackRemove, Direction.ServerToClient)]
public sealed partial class ServerRewardTrackRemove
{
    [UInt(14)] public uint RewardTrackId { get; set; } // if 0 it removes all reward tracks
}
