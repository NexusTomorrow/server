using NexusTomorrow.Network.Message;
using NexusTomorrow.Game.Static.Crafting;

namespace NexusTomorrow.Network.World.Message.Model.Crafting
{
    [Message(GameMessageOpcode.ServerTradeskillSigilResult)]
    public class ServerTradeskillSigilResult : IWritable
    {
        public TradeskillResult TradeskillSigilResult { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(TradeskillSigilResult, 32u);
        }
    }
}
