using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Sets one account item cooldown; the client raises AccountInventoryUpdate.
/// </summary>
[WireMessage(GameMessageOpcode.Server0974, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class AccountItemCooldownChanged
{
    /// <summary>
    /// The cooldown.
    /// </summary>
    public AccountItemCooldown Cooldown { get; set; } = new();
}
