using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

/// <summary>
/// Shares a quest: the request the script call Quest:Share sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client045F, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class QuestShare
{
    /// <summary>
    /// The quest, as its Quest2 row id.
    /// </summary>
    [UInt(15)] public ushort QuestId { get; private set; }
}
