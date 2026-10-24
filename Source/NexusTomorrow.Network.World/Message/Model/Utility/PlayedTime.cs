using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

/// <summary>
/// Answers <see cref="PlayedTimeRequest"/> with the character's creation time and played times. The client turns the four values into
/// lines of text and raises PlayedTime with them for its add-ons, and the chat log shows the lines.
/// </summary>
[WireMessage(GameMessageOpcode.Server0694, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PlayedTime
{
    /// <summary>
    /// When the character was created, as a UTC file time: 100-nanosecond intervals since 1601-01-01, the value
    /// <see cref="DateTime.FromFileTimeUtc(long)"/> reads. The client shows it as a date in the player's local time.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong CreationTime { get; set; }

    /// <summary>
    /// The time played this session, in seconds. The client shows it in hours, minutes and seconds.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint TimePlayedSession { get; set; }

    /// <summary>
    /// The character's total time played, in seconds. The client shows it in days, hours, minutes and seconds.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint TimePlayedTotal { get; set; }

    /// <summary>
    /// The time played at the character's current level, in seconds. The client shows it in days, hours, minutes and seconds.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint TimePlayedLevel { get; set; }
}
