using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// The time left on one account item cooldown group.
/// </summary>
[WireStruct]
public sealed partial class AccountItemCooldown
{
    /// <summary>
    /// The cooldown group, by the id an AccountItem row names in its accountItemCooldownGroupId; add-ons see the time left as the
    /// cooldown of each account item in the group.
    /// </summary>
    [UInt(32)] public uint CooldownGroupId { get; set; }

    /// <summary>
    /// The seconds left on the cooldown, which the client counts from the message's arrival.
    /// </summary>
    [UInt(32)] public uint RemainingSeconds { get; set; }
}
