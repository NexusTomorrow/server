using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

/// <summary>
/// Sets whether a quest is tracked: the request the script calls Quest:SetTracked and Quest:ToggleTracked lead to.
/// </summary>
/// <remarks>
/// The client sends it only for a quest it holds as active. It sets its own tracked mark to match before sending, then raises
/// QuestTrackedChanged.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0364, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class QuestSetTracked
{
    /// <summary>
    /// The quest, as its Quest2 row id.
    /// </summary>
    [UInt(15)] public ushort QuestId { get; private set; }

    /// <summary>
    /// True to track the quest.
    /// </summary>
    [UInt(1)] public bool Tracked { get; private set; }
}
