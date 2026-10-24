using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Sets each listed cooldown; the client raises AccountInventoryUpdate.
/// </summary>
[WireMessage(GameMessageOpcode.Server0969, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class AccountItemCooldowns
{
    /// <summary>
    /// The cooldowns, one per group.
    /// </summary>
    [Array(CountBits = 32)] public List<AccountItemCooldown> Cooldowns { get; set; } = [];
}
