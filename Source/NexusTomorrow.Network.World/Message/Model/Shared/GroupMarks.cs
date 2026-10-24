using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// A group's target marks.
/// </summary>
[WireStruct]
public sealed partial class GroupMarks
{
    /// <summary>
    /// The marks, each a marker and the unit it marks.
    /// </summary>
    [VerifiedMember, Array(CountBits = 32)] public GroupMark[] Marks { get; set; } = [];
}
