using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.ServerTradeskillSigilResult, Direction.ServerToClient)]
public sealed partial class ServerTradeskillSigilResult
{
    [UInt(32)] public TradeskillResult TradeskillSigilResult { get; set; }
}
