using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

/// <summary>
/// Sets whether a quest is ignored: the request the script call Quest:ToggleIgnored leads to.
/// </summary>
/// <remarks>
/// The client sends it only for a quest it knows, and only when the value differs from whether its log holds the quest in
/// state 7.
/// </remarks>
[WireMessage(GameMessageOpcode.Client035E, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class QuestSetIgnored
{
    /// <summary>
    /// The quest, as its Quest2 row id.
    /// </summary>
    [UInt(15)] public ushort QuestId { get; private set; }

    /// <summary>
    /// True to ignore the quest.
    /// </summary>
    [UInt(1)] public bool Ignored { get; private set; }
}
