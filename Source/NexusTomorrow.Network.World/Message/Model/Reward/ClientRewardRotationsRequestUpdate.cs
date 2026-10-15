using NexusTomorrow.Game.Static.Reward;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.ClientRewardRotationsRequestUpdate, Direction.ClientToServer)]
public sealed partial class ClientRewardRotationsRequestUpdate
{
    [UInt(32)] public RewardRotationContentType ContentType { get; private set; }
}
