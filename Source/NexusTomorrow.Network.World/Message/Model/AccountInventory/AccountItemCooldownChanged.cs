using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server0974, Direction.ServerToClient)]
public sealed partial class AccountItemCooldownChanged
{
    public AccountItemCooldown Unknown0 { get; set; } = new();
}
