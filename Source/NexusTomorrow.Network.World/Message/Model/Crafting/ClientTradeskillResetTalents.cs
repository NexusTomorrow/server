using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[Message(GameMessageOpcode.ClientTradeskillResetTalents)]
public class ClientTradeskillResetTalents : IReadable
{
    public TradeskillType TradeskillId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        TradeskillId = reader.ReadEnum<TradeskillType>(32u);
    }
}
