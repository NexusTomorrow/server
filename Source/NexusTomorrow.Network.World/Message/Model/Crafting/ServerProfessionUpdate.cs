using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.ServerProfessionUpdate, Direction.ServerToClient)]
public sealed partial class ServerProfessionUpdate
{
    public TradeskillInfo Tradeskill { get; set; } = new TradeskillInfo();
}
