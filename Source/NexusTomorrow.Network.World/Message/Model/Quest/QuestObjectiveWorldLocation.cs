using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

/// <summary>
/// Sets the world location for one of a quest's objectives.
/// </summary>
/// <remarks>
/// The client stores the location on the objective, only for a quest that has an objective at that index.
/// </remarks>
[WireMessage(GameMessageOpcode.Server012C, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class QuestObjectiveWorldLocation
{
    /// <summary>
    /// The quest, by quest id.
    /// </summary>
    [VerifiedMember, UInt(15)] public uint QuestId { get; set; }

    /// <summary>
    /// The objective's index within the quest; the client ignores an index at or above the quest's objective count.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint ObjectiveIndex { get; set; }

    /// <summary>
    /// The objective's world location value.
    /// </summary>
    [VerifiedMember, UInt(17)] public uint WorldLocation { get; set; }
}
