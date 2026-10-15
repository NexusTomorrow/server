using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.ClientTradeskillLearn, Direction.ClientToServer)]
public sealed partial class ClientTradeskillLearn
{
    [UInt(32)] public TradeskillType ToLearnTradeskillId { get; private set; }
    [UInt(32)] public TradeskillType ToDropTradeskillId { get; private set; }
}
