using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

/// <summary>
/// Sets the progress of one objective of a quest the client holds as active.
/// </summary>
/// <remarks>
/// The client then raises QuestObjectiveUpdated, or ContractObjectiveUpdated when the quest's Quest2 flags carry 0x80000.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0361, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class QuestObjectiveUpdated
{
    /// <summary>
    /// The quest, as its Quest2 row id.
    /// </summary>
    [UInt(15)] public ushort QuestId { get; set; }

    /// <summary>
    /// The objective's position among the quest's objectives; the client ignores a position past the last.
    /// </summary>
    [UInt(32)] public uint ObjectiveIndex { get; set; }

    /// <summary>
    /// The objective's progress count.
    /// </summary>
    [UInt(32)] public uint Progress { get; set; }
}
