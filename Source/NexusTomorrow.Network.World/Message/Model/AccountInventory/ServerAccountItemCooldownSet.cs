using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.ServerAccountItemCooldownSet, Direction.ServerToClient)]
public sealed partial class ServerAccountItemCooldownSet
{
    public AccountItemCooldown Cooldown { get; set; }
}
