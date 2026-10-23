using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Warns that a mentoring pair has moved out of range, or ends the warning.
/// </summary>
/// <remarks>
/// While the player is in a group, the client raises Group_MentorLeftAOI with the time in seconds and the flag.
/// </remarks>
[WireMessage(GameMessageOpcode.Server043A, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GroupMentoringRangeWarning
{
    /// <summary>
    /// How long the pair has to come back in range before mentoring ends, in milliseconds.
    /// </summary>
    [UInt(32)] public uint TimeUntilCancelled { get; set; }

    /// <summary>
    /// True when the countdown has stopped: the pair is back in range, or the time ran out.
    /// </summary>
    [UInt(1)] public bool TimerStopped { get; set; }
}
