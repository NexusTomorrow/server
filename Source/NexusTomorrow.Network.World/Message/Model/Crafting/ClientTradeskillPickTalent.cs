using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.ClientTradeskillPickTalent, Direction.ClientToServer)]
public sealed partial class ClientTradeskillPickTalent
{
    [UInt(32)] public TradeskillType TradeskillId { get; private set; }
    [UInt(32)] public uint Tier { get; private set; }
    [UInt(32)] public uint TradeskillBonusId { get; private set; }
}
