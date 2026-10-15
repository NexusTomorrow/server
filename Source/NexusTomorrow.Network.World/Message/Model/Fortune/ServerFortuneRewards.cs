using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Fortune;

[Message(GameMessageOpcode.ServerFortuneRewards)]
[WireMessage(GameMessageOpcode.ServerFortuneRewards, Direction.ServerToClient)]
public sealed partial class ServerFortuneRewards : IWritable
{
    [WireStruct]
    public sealed partial class MoneyReward : IWritable
    {
        [UInt(5)] public byte SecondaryCurrencyAmount { get; set; }
        [UInt(32)] public uint MoneyAmount { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(SecondaryCurrencyAmount, 5u);
            writer.Write(MoneyAmount);
        }
    }

    [Array(CountBits = 32), UInt(32)] public List<uint> Item2IdRewards { get; set; } = [];
    [Array(CountBits = 32)] public List<MoneyReward> MoneyRewards { get; set; } = [];       // not seen used in sniffs
    [Array(CountBits = 32), Float32] public List<float> RewardItemProbabilities { get; set; } = [];  // not seen used in sniffs
    [Array(CountBits = 32), Float32] public List<float> RewardMoneyProbabilities { get; set; } = []; // not seen used in sniffs

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Item2IdRewards.Count);
        Item2IdRewards.ForEach(reward => writer.Write(reward));
        writer.Write(MoneyRewards.Count);
        MoneyRewards.ForEach(reward => reward.Write(writer));
        writer.Write(RewardItemProbabilities.Count);
        RewardItemProbabilities.ForEach(reward => writer.Write(reward));
        writer.Write(RewardMoneyProbabilities.Count);
        RewardMoneyProbabilities.ForEach(reward => writer.Write(reward));
    }
}
