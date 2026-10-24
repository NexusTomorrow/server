using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Sets the Omnibits bonus the account has earned this week; the client raises AccountCurrencyChanged.
/// </summary>
[WireMessage(GameMessageOpcode.Server096F, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class OmnibitsBonusEarned
{
    /// <summary>
    /// The weekly Omnibits bonus earned so far, which GameLib.GetOmnibitsBonusInfo returns as nWeeklyBonusEarned.
    /// </summary>
    [UInt(32)] public uint WeeklyBonusEarned { get; set; }
}
