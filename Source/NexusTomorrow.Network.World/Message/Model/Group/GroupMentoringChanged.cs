using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Starts or ends a mentoring pair in the player's current group.
/// </summary>
/// <remarks>
/// When one starts, the client raises Group_MentorRelationship with the mentor and the mentee.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0441, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GroupMentoringChanged
{
    /// <summary>
    /// The group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown1 { get; set; }

    /// <summary>
    /// The member who mentors.
    /// </summary>
    public RealmScopedId MentorId { get; set; } = new();

    /// <summary>
    /// The member mentored.
    /// </summary>
    public RealmScopedId MenteeId { get; set; } = new();

    /// <summary>
    /// True when the mentor stops mentoring, false when it starts.
    /// </summary>
    [UInt(1)] public bool Ended { get; set; }
}
