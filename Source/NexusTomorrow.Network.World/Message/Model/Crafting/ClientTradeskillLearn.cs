using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[Message(GameMessageOpcode.ClientTradeskillLearn)]
public class ClientTradeskillLearn : IReadable
{
    public TradeskillType ToLearnTradeskillId { get; private set; }
    public TradeskillType ToDropTradeskillId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ToLearnTradeskillId = reader.ReadEnum<TradeskillType>(32u);
        ToDropTradeskillId = reader.ReadEnum<TradeskillType>(32u);
    }
}
