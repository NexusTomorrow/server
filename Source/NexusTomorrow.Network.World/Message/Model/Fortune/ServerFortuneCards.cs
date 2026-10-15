using NexusTomorrow.Game.Static.Fortune;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Fortune;

// Generally end all 0 after ClientFortunesNotifyGame/Storefront and then send with values after ClientFortunesStart
// Fortune games can be partially played and resumed so this message is not always sent with 0's after ClientFortunesNotifyGame/Storefront
[Message(GameMessageOpcode.ServerFortuneCards)]
[WireMessage(GameMessageOpcode.ServerFortuneCards, Direction.ServerToClient)]
public sealed partial class ServerFortuneCards : IWritable
{
    [UInt(3)] public FortuneOperation Operation { get; set; }
    [FixedArray(3), UInt(2)] public RewardRarity[] Rarity { get; set; } = new RewardRarity[3];
    [FixedArray(3), UInt(32)] public uint[] AccountItemId { get; set; } = new uint[3];
    [FixedArray(3), UInt(1)] public bool[] CardFlipped { get; set; } = new bool[3];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Operation, 3u);
        for (uint i = 0; i < 3; i++)
        {
            writer.Write(Rarity[i], 2u);
        }
        for (uint i = 0; i < 3; i++)
        {
            writer.Write(AccountItemId[i]);
        }
        for (uint i = 0; i < 3; i++)
        {
            writer.Write(CardFlipped[i]);
        }
    }
}
