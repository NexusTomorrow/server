using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[Message(GameMessageOpcode.ServerTradeskillSigilResult)]
[WireMessage(GameMessageOpcode.ServerTradeskillSigilResult, Direction.ServerToClient)]
public sealed partial class ServerTradeskillSigilResult : IWritable
{
    [UInt(32)] public TradeskillResult TradeskillSigilResult { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TradeskillSigilResult, 32u);
    }
}
