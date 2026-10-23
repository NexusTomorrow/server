using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Dismisses the warning that a mentoring pair is out of range: the request GroupLib.CloseMentoringAOIDialog sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client0439, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GroupCloseMentoringRangeWarning
{
    /// <summary>
    /// The player's current group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The player's own member.
    /// </summary>
    public RealmScopedId PlayerId { get; set; } = new();
}
