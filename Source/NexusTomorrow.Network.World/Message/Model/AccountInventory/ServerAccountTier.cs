using NexusTomorrow.Game.Static;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.ServerAccountTier, Direction.ServerToClient)]
public sealed partial class ServerAccountTier
{
    [UInt(5)] public AccountTier Tier { get; set; }
}
