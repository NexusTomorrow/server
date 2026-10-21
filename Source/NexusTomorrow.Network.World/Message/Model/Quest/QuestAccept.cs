using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

/// <summary>
/// Accepts a quest: the request the script call Contract:Accept sends, among others.
/// </summary>
/// <remarks>
/// For a quest the client's log holds in state 4 its senders send <see cref="QuestReaccept"/> instead.
/// </remarks>
[WireMessage(GameMessageOpcode.Client035B, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class QuestAccept
{
    public Server0111Part2 Unknown0 { get; } = new();

    /// <summary>
    /// The quest, as its Quest2 row id.
    /// </summary>
    [UInt(15)] public ushort QuestId { get; private set; }
}
