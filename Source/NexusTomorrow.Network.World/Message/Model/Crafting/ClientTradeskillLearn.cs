using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[Message(GameMessageOpcode.ClientTradeskillLearn)]
[WireMessage(GameMessageOpcode.ClientTradeskillLearn, Direction.ClientToServer)]
public sealed partial class ClientTradeskillLearn : IReadable
{
    [UInt(32)] public TradeskillType ToLearnTradeskillId { get; private set; }
    [UInt(32)] public TradeskillType ToDropTradeskillId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ToLearnTradeskillId = reader.ReadEnum<TradeskillType>(32u);
        ToDropTradeskillId = reader.ReadEnum<TradeskillType>(32u);
    }
}
