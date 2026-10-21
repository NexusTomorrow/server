using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

/// <summary>
/// Abandons a quest: the request the script call Quest:Abandon sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client035A, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class QuestAbandon
{
    /// <summary>
    /// The quest, as its Quest2 row id.
    /// </summary>
    [UInt(15)] public ushort QuestId { get; private set; }
}
