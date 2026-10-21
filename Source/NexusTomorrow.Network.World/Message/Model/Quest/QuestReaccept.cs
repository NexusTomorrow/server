using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

/// <summary>
/// The accept request for a quest the client's log holds in state 4: sent in place of <see cref="QuestAccept"/> by each of its
/// senders, the script call Contract:Accept among them.
/// </summary>
[WireMessage(GameMessageOpcode.Client0365, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class QuestReaccept
{
    /// <summary>
    /// The quest, as its Quest2 row id.
    /// </summary>
    [UInt(15)] public ushort QuestId { get; private set; }
}
