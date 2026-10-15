using NexusTomorrow.Game.Static.Reward;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[Message(GameMessageOpcode.ClientRewardRotationsRequestUpdate)]
[WireMessage(GameMessageOpcode.ClientRewardRotationsRequestUpdate, Direction.ClientToServer)]
public sealed partial class ClientRewardRotationsRequestUpdate : IReadable
{
    [UInt(32)] public RewardRotationContentType ContentType { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ContentType = reader.ReadEnum<RewardRotationContentType>(32u);
    }
}
