using NexusTomorrow.Game.Static.AccountInventory;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Sets the account's login days, the rewards it has had and the premium lockbox key's state; the client raises DailyLoginUpdate.
/// </summary>
[WireMessage(GameMessageOpcode.Server096E, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class DailyLoginStatus
{
    /// <summary>
    /// How many days the account has logged in, in all; the client adds one at each new login day.
    /// </summary>
    [UInt(32)] public uint LoginDaysTotal { get; set; }

    [UInt(32)] public uint Unknown1 { get; set; }

    /// <summary>
    /// The last login day rewarded; the rewards for the days after it, up to <see cref="LoginDaysTotal"/>, are due.
    /// </summary>
    [UInt(32)] public uint LastRewardedLoginDay { get; set; }

    [UInt(32)] public uint Unknown3 { get; set; }

    /// <summary>
    /// The milliseconds until the next login day starts; from then the client starts a new login day every 24 hours.
    /// </summary>
    [UInt(32)] public uint NextDayDelayMs { get; set; }

    /// <summary>
    /// The days until the next premium lockbox key, which the client counts from the message's arrival.
    /// </summary>
    [Float32] public float DaysUntilNextKey { get; set; }

    /// <summary>
    /// The premium lockbox key's status.
    /// </summary>
    [UInt(3)] public PremiumLockboxKeyStatus KeyStatus { get; set; }
}
