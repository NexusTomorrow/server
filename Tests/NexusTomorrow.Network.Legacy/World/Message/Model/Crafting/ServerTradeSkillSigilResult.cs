using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[Message(GameMessageOpcode.ServerTradeskillSigilResult)]
public class ServerTradeskillSigilResult : IWritable
{
    public TradeskillResult TradeskillSigilResult { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TradeskillSigilResult, 32u);
    }
}
