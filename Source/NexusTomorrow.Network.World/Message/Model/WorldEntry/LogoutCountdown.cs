using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.WorldEntry;

/// <summary>
/// Starts the countdown before a logout completes.
/// </summary>
[WireMessage(GameMessageOpcode.Server0092, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class LogoutCountdown
{
    /// <summary>
    /// The Signature bonuses the player earned this session, as the exit window lists them.
    /// </summary>
    [WireStruct]
    public sealed partial class LogoutSignatureBonuses
    {
        /// <summary>
        /// The session's Signature experience, which GameLib.GetGameExitInfo reports in tSignatureBonuses as nXp.
        /// </summary>
        [VerifiedMember, UInt(32)] public uint Experience { get; set; }

        /// <summary>
        /// The session's Signature elder points, which GameLib.GetGameExitInfo reports in tSignatureBonuses as nElderPoints.
        /// </summary>
        [VerifiedMember, UInt(32)] public uint ElderPoints { get; set; }

        /// <summary>
        /// The session's Signature amount of each currency: entry i is the amount of currency type i, which GameLib.GetGameExitInfo
        /// reports under key i of tSignatureBonuses' tCurrencies. The client leaves out an entry of 0.
        /// </summary>
        [VerifiedMember, FixedArray(15), UInt(64)] public ulong[] Currencies { get; set; } = new ulong[15];

        /// <summary>
        /// The session's Signature amount of each account currency: entry i is the amount of account currency i, which
        /// GameLib.GetGameExitInfo reports under key i of tSignatureBonuses' tAccountCurrencies. The client leaves out an entry of 0.
        /// </summary>
        [VerifiedMember, FixedArray(19), UInt(64)] public ulong[] AccountCurrencies { get; set; } = new ulong[19];
    }

    /// <summary>
    /// How long the countdown runs, in milliseconds. The client sets its deadline that far ahead, GameLib.GetGameExitInfo reports what
    /// remains as fTimeRemaining, which the exit window counts down, and at the deadline the client goes on with the logout
    /// (<see cref="LogoutConfirm"/>).
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Duration { get; set; }

    [UInt(1)] public bool Unknown1 { get; set; }

    /// <summary>
    /// The Signature bonuses the exit window shows for the session. A server that tracks none sends zeros, which list nothing.
    /// </summary>
    [VerifiedMember] public LogoutSignatureBonuses SignatureBonuses { get; set; } = new();
}
