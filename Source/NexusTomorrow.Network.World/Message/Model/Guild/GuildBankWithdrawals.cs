using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// What the player has taken from a guild bank today.
/// </summary>
[WireStruct]
public sealed partial class GuildBankWithdrawals
{
    /// <summary>
    /// The money withdrawn today.
    /// </summary>
    [UInt(64)] public ulong MoneyToday { get; set; }

    /// <summary>
    /// The items withdrawn today, a count for each tab in tab order.
    /// </summary>
    [FixedArray(10), UInt(32)] public uint[] ItemsToday { get; set; } = new uint[10];

    /// <summary>
    /// The money spent on repairs today; GuildTypeLib's GetBankMoneyRepairRemaining is the rank's repair limit less this.
    /// </summary>
    [UInt(64)] public ulong RepairToday { get; set; }
}
