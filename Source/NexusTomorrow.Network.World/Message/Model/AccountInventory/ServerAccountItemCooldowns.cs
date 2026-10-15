using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.ServerAccountItemCooldowns, Direction.ServerToClient)]
public sealed partial class ServerAccountItemCooldowns
{
    [Array(CountBits = 32)] public List<AccountItemCooldown> AccountItemCooldowns { get; set; } = [];
}
