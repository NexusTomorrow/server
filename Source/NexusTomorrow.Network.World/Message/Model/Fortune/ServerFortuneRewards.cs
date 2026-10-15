using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Fortune;

[WireMessage(GameMessageOpcode.ServerFortuneRewards, Direction.ServerToClient)]
public sealed partial class ServerFortuneRewards
{
    [WireStruct]
    public sealed partial class MoneyReward
    {
        [UInt(5)] public byte SecondaryCurrencyAmount { get; set; }
        [UInt(32)] public uint MoneyAmount { get; set; }
    }

    [Array(CountBits = 32), UInt(32)] public List<uint> Item2IdRewards { get; set; } = [];
    [Array(CountBits = 32)] public List<MoneyReward> MoneyRewards { get; set; } = [];       // not seen used in sniffs
    [Array(CountBits = 32), Float32] public List<float> RewardItemProbabilities { get; set; } = [];  // not seen used in sniffs
    [Array(CountBits = 32), Float32] public List<float> RewardMoneyProbabilities { get; set; } = []; // not seen used in sniffs
}
