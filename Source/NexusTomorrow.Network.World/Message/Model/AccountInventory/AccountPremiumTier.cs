using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Sets the account's premium tier; when the tier changes, the client raises PremiumTierChanged with it.
/// </summary>
[WireMessage(GameMessageOpcode.Server097F, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class AccountPremiumTier
{
    /// <summary>
    /// The premium tier, as a number; PremiumTierChanged carries it as nTier.
    /// </summary>
    [UInt(5)] public byte Tier { get; set; }
}
