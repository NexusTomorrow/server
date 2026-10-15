using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.ClientTradeskillResetTalents, Direction.ClientToServer)]
public sealed partial class ClientTradeskillResetTalents
{
    [UInt(32)] public TradeskillType TradeskillId { get; private set; }
}
