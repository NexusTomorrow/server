using NexusTomorrow.Game.Static.Reward;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward
{
    [Message(GameMessageOpcode.ClientRewardRotationsRequestUpdate)]
    public class ClientRewardRotationsRequestUpdate : IReadable
    {
        public RewardRotationContentType ContentType { get; private set; }

        public void Read(GamePacketReader reader)
        {
            ContentType = reader.ReadEnum<RewardRotationContentType>(32u);
        }
    }
}
