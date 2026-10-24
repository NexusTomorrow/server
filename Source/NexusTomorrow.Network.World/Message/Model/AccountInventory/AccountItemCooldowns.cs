using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server0969, Direction.ServerToClient)]
public sealed partial class AccountItemCooldowns
{
    [Array(CountBits = 32)] public List<AccountItemCooldown> Unknown0 { get; set; } = [];
}
